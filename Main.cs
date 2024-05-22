using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private void Main_Load(object sender, EventArgs e)
        {
            kill_ffmpeg();//runing this ti kill all previous instace of ffmpeg.exe
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
                    CreateNoWindow = true
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

        private async Task WaitForExitAsync(Process process)
        {
            var tcs = new TaskCompletionSource<int>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(process.ExitCode);
            if (!process.HasExited)
            {
                await tcs.Task;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
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

            // Set progress bar properties
            progressBar.Minimum = 0;
            progressBar.Maximum = audioFiles.Count;
            progressBar.Value = 0;

            foreach (string file in audioFiles)
            {
                try
                {
                    txtStatus.Text = $"Processing: {Path.GetFileName(file)}";
                    Application.DoEvents(); // Update the UI

                    string outputFilePath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(file) + "." + selectedFormat);
                    ConvertToFormat(file, outputFilePath, selectedFormat);

                    // Increment progress bar value
                    progressBar.Value += 1;
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error converting {Path.GetFileName(file)}: {ex.Message}");
                }
            }

            txtStatus.Text = "Conversion completed."; 
            kill_ffmpeg();
        }

        private void kill_ffmpeg()
        {

            // Get all instances of processes named "ffmpeg"
            var ffmpegProcesses = Process.GetProcessesByName("ffmpeg");

            foreach (var process in ffmpegProcesses)
            {
                try
                {
                    // Kill the process
                    process.Kill();
                    // Optional: Wait for the process to exit
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., process might have already exited)
                    MessageBox.Show($"Failed to kill process {process.Id}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MessageBox.Show("All ffmpeg processes have been killed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }





        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            kill_ffmpeg();
        }
    }
}
