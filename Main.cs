using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kya_karu
{
    public partial class Main : Form
    {
        private List<string> audioFiles;
        private string outputFolder;
        private System.Windows.Forms.ProgressBar progressBar;
        private List<Process> ffmpegProcesses;

        public Main()
        {
            audioFiles = new List<string>();
            InitializeComponent();
            progressBar = new System.Windows.Forms.ProgressBar();
            ffmpegProcesses = new List<Process>(); // Initialize the list
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            dependencies_check();
            await KillFFmpegAsync();//runing this ti kill all previous instace of ffmpeg.exe
            // Check if the application is running as administrator
            if (!IsRunningAsAdmin())
            {
                MessageBox.Show("Oh no. It looks like this app does not have admin rights. Please wait while we restart it as an administrator for full functionality.");

                // If not, restart the application with admin rights
                RunAsAdmin();
                Application.Exit();
            }
            else
            {
                // Application is running as administrator
                
            }
        }


        private void dependencies_check()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string ffmpegPath = Path.Combine(currentDirectory, "ffmpeg.exe");

            if (File.Exists(ffmpegPath))
            {
                MessageBox.Show("ffmpeg.exe exists in the Application directory.", "File Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ffmpeg.exe does not exist in the Application directory.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool IsRunningAsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void RunAsAdmin()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = Application.ExecutablePath;
            startInfo.Verb = "runas"; // This will prompt the UAC elevation dialog
            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while trying to restart as administrator: " + ex.Message);
            }
        }

        private void btnSelectFiles_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "Audio Files|*.mp3;*.wma;*.wav;*.flac";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    listBoxFiles.Items.Clear();
                    audioFiles.Clear();
                    foreach (string file in openFileDialog.FileNames)
                    {
                        audioFiles.Add(file);
                        listBoxFiles.Items.Add(Path.GetFileName(file));
                    }
                }
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    outputFolder = folderBrowserDialog.SelectedPath;
                    lblSelectedFolder.Text = "Selected folder: " + outputFolder;
                }
            }
        }

        private async Task ConvertToFormat(string inputFilePath, string outputFilePath, string format)
        {
            string ffmpegPath = Path.Combine(Application.StartupPath, "ffmpeg.exe");
            string arguments = $"-i \"{inputFilePath}\" \"{outputFilePath}\"";

            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };

                using (var process = Process.Start(processStartInfo))
                {
                    ffmpegProcesses.Add(process); // Add the process to the list

                    var output = await process.StandardOutput.ReadToEndAsync();
                    var error = await process.StandardError.ReadToEndAsync();

                    await WaitForExitAsync(process);

                    if (process.ExitCode == 0)
                    {
                        Console.WriteLine($"Conversion successful: {output}");
                    }
                    else
                    {
                        Console.WriteLine($"Conversion failed: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during conversion: {ex.Message}");
            }
        }

        private Task WaitForExitAsync(Process process)
        {
            var tcs = new TaskCompletionSource<bool>();

            // If the process has already exited, set the result immediately
            if (process.HasExited)
            {
                tcs.SetResult(true);
            }
            else
            {
                process.EnableRaisingEvents = true;
                process.Exited += (sender, args) => tcs.TrySetResult(true);
            }

            return tcs.Task;
        }


        private async void btnConvert_Click(object sender, EventArgs e)
        {
            if (audioFiles.Count == 0)
            {
                MessageBox.Show("Please select some audio files first.");
                return;
            }

            if (string.IsNullOrEmpty(outputFolder))
            {
                MessageBox.Show("Please select a destination folder.");
                return;
            }

            string selectedFormat = comboBoxFormats.SelectedItem.ToString().ToLower();

            foreach (string file in audioFiles)
            {
                try
                {
                    txtStatus.Text = $"Processing: {Path.GetFileName(file)}";
                    Application.DoEvents(); // Update the UI

                    string outputFilePath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(file) + "." + selectedFormat);
                    await ConvertToFormat(file, outputFilePath, selectedFormat); // Await the conversion

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error converting {Path.GetFileName(file)}: {ex.Message}");
                }
            }

            txtStatus.Text = "Conversion completed.";
        }


        private async Task KillFFmpegAsync()
        {
            var ffmpegProcesses = Process.GetProcessesByName("ffmpeg");

            var tasks = new List<Task>();

            foreach (var process in ffmpegProcesses)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        if (!process.HasExited)
                        {
                            // Kill the process asynchronously
                            process.Kill();

                            // Wait for the process to exit asynchronously
                            await Task.Run(() => process.WaitForExit());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to kill process {process.Id}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }));
            }

            // Wait for all processes to be killed asynchronously
            await Task.WhenAll(tasks);
        }






        private async void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            await KillFFmpegAsync();
        }
    }
}
