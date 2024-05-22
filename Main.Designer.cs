namespace kya_karu
{
    partial class Main
    {
        private System.Windows.Forms.ComboBox comboBoxFormats;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Label lblSelectedFolder;
        private System.Windows.Forms.Button btnSelectFiles;
        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Label txtStatus;


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.comboBoxFormats = new System.Windows.Forms.ComboBox();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.lblSelectedFolder = new System.Windows.Forms.Label();
            this.btnSelectFiles = new System.Windows.Forms.Button();
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // comboBoxFormats
            // 
            this.comboBoxFormats.FormattingEnabled = true;
            this.comboBoxFormats.Items.AddRange(new object[] {
            "WAV",
            "MP3",
            "FLAC",
            "WMA"});
            this.comboBoxFormats.Location = new System.Drawing.Point(13, 13);
            this.comboBoxFormats.Name = "comboBoxFormats";
            this.comboBoxFormats.Size = new System.Drawing.Size(121, 24);
            this.comboBoxFormats.TabIndex = 0;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(140, 13);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(173, 24);
            this.btnSelectFolder.TabIndex = 1;
            this.btnSelectFolder.Text = "Select Destination Folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // lblSelectedFolder
            // 
            this.lblSelectedFolder.Location = new System.Drawing.Point(13, 40);
            this.lblSelectedFolder.Name = "lblSelectedFolder";
            this.lblSelectedFolder.Size = new System.Drawing.Size(248, 23);
            this.lblSelectedFolder.TabIndex = 2;
            this.lblSelectedFolder.Text = "No folder selected";
            // 
            // btnSelectFiles
            // 
            this.btnSelectFiles.Location = new System.Drawing.Point(13, 66);
            this.btnSelectFiles.Name = "btnSelectFiles";
            this.btnSelectFiles.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFiles.TabIndex = 3;
            this.btnSelectFiles.Text = "Select Files";
            this.btnSelectFiles.UseVisualStyleBackColor = true;
            this.btnSelectFiles.Click += new System.EventHandler(this.btnSelectFiles_Click);
            // 
            // listBoxFiles
            // 
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.ItemHeight = 16;
            this.listBoxFiles.Location = new System.Drawing.Point(13, 95);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.Size = new System.Drawing.Size(292, 164);
            this.listBoxFiles.TabIndex = 4;
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(230, 66);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 5;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(13, 262);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(259, 23);
            this.txtStatus.TabIndex = 6;
            this.txtStatus.Text = "Status";
            // 
            // Main
            // 
            this.ClientSize = new System.Drawing.Size(317, 322);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.listBoxFiles);
            this.Controls.Add(this.btnSelectFiles);
            this.Controls.Add(this.lblSelectedFolder);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.comboBoxFormats);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Audio Converter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components;
    }
}
