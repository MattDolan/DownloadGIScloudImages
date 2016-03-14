namespace DownloadImages
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button2 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cmdExit = new System.Windows.Forms.Button();
            this.lstLocation = new System.Windows.Forms.ListBox();
            this.lstNoLocation = new System.Windows.Forms.ListBox();
            this.chkExplorer = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lstNoPic = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(16, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(160, 32);
            this.button2.TabIndex = 1;
            this.button2.Text = "Load GIScloud Photo List";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cmdExit
            // 
            this.cmdExit.Location = new System.Drawing.Point(440, 456);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(96, 32);
            this.cmdExit.TabIndex = 2;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // lstLocation
            // 
            this.lstLocation.FormattingEnabled = true;
            this.lstLocation.Location = new System.Drawing.Point(16, 74);
            this.lstLocation.Name = "lstLocation";
            this.lstLocation.Size = new System.Drawing.Size(160, 368);
            this.lstLocation.TabIndex = 3;
            this.lstLocation.DoubleClick += new System.EventHandler(this.lstLocation_DoubleClick);
            // 
            // lstNoLocation
            // 
            this.lstNoLocation.FormattingEnabled = true;
            this.lstNoLocation.Location = new System.Drawing.Point(196, 74);
            this.lstNoLocation.Name = "lstNoLocation";
            this.lstNoLocation.Size = new System.Drawing.Size(160, 368);
            this.lstNoLocation.TabIndex = 4;
            this.lstNoLocation.DoubleClick += new System.EventHandler(this.lstNoLocation_DoubleClick);
            // 
            // chkExplorer
            // 
            this.chkExplorer.AutoSize = true;
            this.chkExplorer.Checked = true;
            this.chkExplorer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExplorer.Location = new System.Drawing.Point(16, 464);
            this.chkExplorer.Name = "chkExplorer";
            this.chkExplorer.Size = new System.Drawing.Size(169, 17);
            this.chkExplorer.TabIndex = 5;
            this.chkExplorer.Text = "Open Explorer After Download";
            this.chkExplorer.UseVisualStyleBackColor = true;
            this.chkExplorer.CheckedChanged += new System.EventHandler(this.chkExplorer_CheckedChanged);
            this.chkExplorer.Click += new System.EventHandler(this.chkExplorer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Has Location";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Missing Location";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(200, 21);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(152, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // lstNoPic
            // 
            this.lstNoPic.FormattingEnabled = true;
            this.lstNoPic.Location = new System.Drawing.Point(376, 74);
            this.lstNoPic.Name = "lstNoPic";
            this.lstNoPic.Size = new System.Drawing.Size(160, 368);
            this.lstNoPic.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(376, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Location With no Picture";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 502);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstNoPic);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkExplorer);
            this.Controls.Add(this.lstNoLocation);
            this.Controls.Add(this.lstLocation);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.button2);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QC Data - Download GIScloud Images";
            this.Load += new System.EventHandler(this.frmMain_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.ListBox lstLocation;
        private System.Windows.Forms.ListBox lstNoLocation;
        private System.Windows.Forms.CheckBox chkExplorer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ListBox lstNoPic;
        private System.Windows.Forms.Label label3;
    }
}

