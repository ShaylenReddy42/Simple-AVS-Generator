namespace Simple_AVS_Generator
{
    partial class MainForm
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
            this.txbInFile = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.txbOutFile = new System.Windows.Forms.TextBox();
            this.btnOutDir = new System.Windows.Forms.Button();
            this.btnGen = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbAudioCodec = new System.Windows.Forms.ComboBox();
            this.cmbVideoCodec = new System.Windows.Forms.ComboBox();
            this.cbxAudio = new System.Windows.Forms.CheckBox();
            this.cbxVideo = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbxMKV = new System.Windows.Forms.CheckBox();
            this.cbxMP4 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbBitrate = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbChannels = new System.Windows.Forms.ComboBox();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbltitle = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblMinimize = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbSourceFPS = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbKeyframeInterval = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbInFile
            // 
            this.txbInFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.txbInFile.ForeColor = System.Drawing.Color.White;
            this.txbInFile.Location = new System.Drawing.Point(12, 38);
            this.txbInFile.Name = "txbInFile";
            this.txbInFile.ReadOnly = true;
            this.txbInFile.Size = new System.Drawing.Size(560, 20);
            this.txbInFile.TabIndex = 0;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFile.ForeColor = System.Drawing.Color.White;
            this.btnOpenFile.Location = new System.Drawing.Point(578, 36);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(105, 23);
            this.btnOpenFile.TabIndex = 1;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = false;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // txbOutFile
            // 
            this.txbOutFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.txbOutFile.ForeColor = System.Drawing.Color.White;
            this.txbOutFile.Location = new System.Drawing.Point(11, 193);
            this.txbOutFile.Name = "txbOutFile";
            this.txbOutFile.ReadOnly = true;
            this.txbOutFile.Size = new System.Drawing.Size(672, 20);
            this.txbOutFile.TabIndex = 4;
            // 
            // btnOutDir
            // 
            this.btnOutDir.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnOutDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutDir.ForeColor = System.Drawing.Color.White;
            this.btnOutDir.Location = new System.Drawing.Point(577, 219);
            this.btnOutDir.Name = "btnOutDir";
            this.btnOutDir.Size = new System.Drawing.Size(106, 23);
            this.btnOutDir.TabIndex = 5;
            this.btnOutDir.Text = "Output";
            this.btnOutDir.UseVisualStyleBackColor = false;
            this.btnOutDir.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // btnGen
            // 
            this.btnGen.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnGen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGen.ForeColor = System.Drawing.Color.White;
            this.btnGen.Location = new System.Drawing.Point(288, 219);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(127, 23);
            this.btnGen.TabIndex = 6;
            this.btnGen.Text = "Generate";
            this.btnGen.UseVisualStyleBackColor = false;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnNew.Enabled = false;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(11, 219);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(98, 23);
            this.btnNew.TabIndex = 9;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBox1.Controls.Add(this.cmbAudioCodec);
            this.groupBox1.Controls.Add(this.cmbVideoCodec);
            this.groupBox1.Controls.Add(this.cbxAudio);
            this.groupBox1.Controls.Add(this.cbxVideo);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 123);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Encoders";
            // 
            // cmbAudioCodec
            // 
            this.cmbAudioCodec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cmbAudioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAudioCodec.Enabled = false;
            this.cmbAudioCodec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbAudioCodec.ForeColor = System.Drawing.Color.White;
            this.cmbAudioCodec.FormattingEnabled = true;
            this.cmbAudioCodec.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmbAudioCodec.Location = new System.Drawing.Point(19, 92);
            this.cmbAudioCodec.Name = "cmbAudioCodec";
            this.cmbAudioCodec.Size = new System.Drawing.Size(113, 21);
            this.cmbAudioCodec.TabIndex = 3;
            this.cmbAudioCodec.SelectedIndexChanged += new System.EventHandler(this.cmbAudioCodec_SelectedIndexChanged);
            // 
            // cmbVideoCodec
            // 
            this.cmbVideoCodec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cmbVideoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVideoCodec.Enabled = false;
            this.cmbVideoCodec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbVideoCodec.ForeColor = System.Drawing.Color.White;
            this.cmbVideoCodec.FormattingEnabled = true;
            this.cmbVideoCodec.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmbVideoCodec.Location = new System.Drawing.Point(19, 42);
            this.cmbVideoCodec.Name = "cmbVideoCodec";
            this.cmbVideoCodec.Size = new System.Drawing.Size(113, 21);
            this.cmbVideoCodec.TabIndex = 2;
            this.cmbVideoCodec.SelectedIndexChanged += new System.EventHandler(this.cmbVideoCodec_SelectedIndexChanged);
            // 
            // cbxAudio
            // 
            this.cbxAudio.AutoSize = true;
            this.cbxAudio.Enabled = false;
            this.cbxAudio.ForeColor = System.Drawing.Color.White;
            this.cbxAudio.Location = new System.Drawing.Point(19, 69);
            this.cbxAudio.Name = "cbxAudio";
            this.cbxAudio.Size = new System.Drawing.Size(53, 17);
            this.cbxAudio.TabIndex = 1;
            this.cbxAudio.Text = "Audio";
            this.cbxAudio.UseVisualStyleBackColor = true;
            this.cbxAudio.CheckedChanged += new System.EventHandler(this.cbxAudio_CheckedChanged);
            // 
            // cbxVideo
            // 
            this.cbxVideo.AutoSize = true;
            this.cbxVideo.Enabled = false;
            this.cbxVideo.ForeColor = System.Drawing.Color.White;
            this.cbxVideo.Location = new System.Drawing.Point(19, 19);
            this.cbxVideo.Name = "cbxVideo";
            this.cbxVideo.Size = new System.Drawing.Size(53, 17);
            this.cbxVideo.TabIndex = 0;
            this.cbxVideo.Text = "Video";
            this.cbxVideo.UseVisualStyleBackColor = true;
            this.cbxVideo.CheckedChanged += new System.EventHandler(this.cbxVideo_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBox2.Controls.Add(this.cbxMKV);
            this.groupBox2.Controls.Add(this.cbxMP4);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(535, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(148, 122);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output Containers";
            // 
            // cbxMKV
            // 
            this.cbxMKV.AutoSize = true;
            this.cbxMKV.Enabled = false;
            this.cbxMKV.ForeColor = System.Drawing.Color.White;
            this.cbxMKV.Location = new System.Drawing.Point(50, 73);
            this.cbxMKV.Name = "cbxMKV";
            this.cbxMKV.Size = new System.Drawing.Size(49, 17);
            this.cbxMKV.TabIndex = 1;
            this.cbxMKV.Text = "MKV";
            this.cbxMKV.UseVisualStyleBackColor = true;
            this.cbxMKV.CheckedChanged += new System.EventHandler(this.cbxMKV_CheckedChanged);
            // 
            // cbxMP4
            // 
            this.cbxMP4.AutoSize = true;
            this.cbxMP4.Enabled = false;
            this.cbxMP4.ForeColor = System.Drawing.Color.White;
            this.cbxMP4.Location = new System.Drawing.Point(50, 37);
            this.cbxMP4.Name = "cbxMP4";
            this.cbxMP4.Size = new System.Drawing.Size(48, 17);
            this.cbxMP4.TabIndex = 0;
            this.cbxMP4.Text = "MP4";
            this.cbxMP4.UseVisualStyleBackColor = true;
            this.cbxMP4.CheckedChanged += new System.EventHandler(this.cbxMP4_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBox3.Controls.Add(this.cmbBitrate);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.cmbChannels);
            this.groupBox3.Controls.Add(this.cmbLanguage);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(348, 66);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(181, 121);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Audio Options";
            // 
            // cmbBitrate
            // 
            this.cmbBitrate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cmbBitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBitrate.Enabled = false;
            this.cmbBitrate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbBitrate.ForeColor = System.Drawing.Color.White;
            this.cmbBitrate.FormattingEnabled = true;
            this.cmbBitrate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmbBitrate.Location = new System.Drawing.Point(74, 89);
            this.cmbBitrate.Name = "cmbBitrate";
            this.cmbBitrate.Size = new System.Drawing.Size(100, 21);
            this.cmbBitrate.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(7, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Bitrate:";
            // 
            // cmbChannels
            // 
            this.cmbChannels.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cmbChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChannels.Enabled = false;
            this.cmbChannels.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbChannels.ForeColor = System.Drawing.Color.White;
            this.cmbChannels.FormattingEnabled = true;
            this.cmbChannels.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmbChannels.Location = new System.Drawing.Point(74, 56);
            this.cmbChannels.Name = "cmbChannels";
            this.cmbChannels.Size = new System.Drawing.Size(100, 21);
            this.cmbChannels.TabIndex = 3;
            this.cmbChannels.SelectedIndexChanged += new System.EventHandler(this.cmbChannels_SelectedIndexChanged);
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.Enabled = false;
            this.cmbLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbLanguage.ForeColor = System.Drawing.Color.White;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmbLanguage.Location = new System.Drawing.Point(74, 23);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(100, 21);
            this.cmbLanguage.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Channels: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Language: ";
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(12, 9);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(136, 17);
            this.lbltitle.TabIndex = 15;
            this.lbltitle.Text = "Simple AVS Generator";
            // 
            // lblClose
            // 
            this.lblClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClose.ForeColor = System.Drawing.Color.White;
            this.lblClose.Location = new System.Drawing.Point(658, 4);
            this.lblClose.Margin = new System.Windows.Forms.Padding(5);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(25, 25);
            this.lblClose.TabIndex = 16;
            this.lblClose.Text = "X";
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            this.lblClose.MouseEnter += new System.EventHandler(this.lblClose_MouseEnter);
            this.lblClose.MouseLeave += new System.EventHandler(this.lblClose_MouseLeave);
            // 
            // lblMinimize
            // 
            this.lblMinimize.ForeColor = System.Drawing.Color.White;
            this.lblMinimize.Location = new System.Drawing.Point(625, 4);
            this.lblMinimize.Name = "lblMinimize";
            this.lblMinimize.Size = new System.Drawing.Size(25, 25);
            this.lblMinimize.TabIndex = 17;
            this.lblMinimize.Text = "_";
            this.lblMinimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMinimize.Click += new System.EventHandler(this.lblMinimize_Click);
            this.lblMinimize.MouseEnter += new System.EventHandler(this.lblMinimize_MouseEnter);
            this.lblMinimize.MouseLeave += new System.EventHandler(this.lblMinimize_MouseLeave);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cmbKeyframeInterval);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.cmbSourceFPS);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(173, 66);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(169, 121);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Video Options";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Select Source Framerate";
            // 
            // cmbSourceFPS
            // 
            this.cmbSourceFPS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cmbSourceFPS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceFPS.Enabled = false;
            this.cmbSourceFPS.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbSourceFPS.ForeColor = System.Drawing.Color.White;
            this.cmbSourceFPS.FormattingEnabled = true;
            this.cmbSourceFPS.Location = new System.Drawing.Point(23, 41);
            this.cmbSourceFPS.Name = "cmbSourceFPS";
            this.cmbSourceFPS.Size = new System.Drawing.Size(121, 21);
            this.cmbSourceFPS.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Select Keyframe Interval ";
            // 
            // cmbKeyframeInterval
            // 
            this.cmbKeyframeInterval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cmbKeyframeInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyframeInterval.Enabled = false;
            this.cmbKeyframeInterval.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbKeyframeInterval.ForeColor = System.Drawing.Color.White;
            this.cmbKeyframeInterval.FormattingEnabled = true;
            this.cmbKeyframeInterval.Location = new System.Drawing.Point(23, 91);
            this.cmbKeyframeInterval.Name = "cmbKeyframeInterval";
            this.cmbKeyframeInterval.Size = new System.Drawing.Size(121, 21);
            this.cmbKeyframeInterval.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(697, 255);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.lblMinimize);
            this.Controls.Add(this.lblClose);
            this.Controls.Add(this.lbltitle);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnGen);
            this.Controls.Add(this.btnOutDir);
            this.Controls.Add(this.txbOutFile);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.txbInFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Simple AVS Generator";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbInFile;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox txbOutFile;
        private System.Windows.Forms.Button btnOutDir;
        private System.Windows.Forms.Button btnGen;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbxMKV;
        private System.Windows.Forms.CheckBox cbxMP4;
        private System.Windows.Forms.ComboBox cmbVideoCodec;
        private System.Windows.Forms.CheckBox cbxVideo;
        private System.Windows.Forms.CheckBox cbxAudio;
        private System.Windows.Forms.ComboBox cmbAudioCodec;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbChannels;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbBitrate;
        private System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblMinimize;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cmbSourceFPS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbKeyframeInterval;
        private System.Windows.Forms.Label label5;
    }
}

