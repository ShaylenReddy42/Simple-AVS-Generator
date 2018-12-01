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
            this.cbxAudioCodec = new System.Windows.Forms.ComboBox();
            this.cbxVideoCodec = new System.Windows.Forms.ComboBox();
            this.chkAEnc = new System.Windows.Forms.CheckBox();
            this.chkVEnc = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbMKV = new System.Windows.Forms.CheckBox();
            this.chbMP4 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbxChannels = new System.Windows.Forms.ComboBox();
            this.cbxLanguage = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbInFile
            // 
            this.txbInFile.Enabled = false;
            this.txbInFile.Location = new System.Drawing.Point(13, 13);
            this.txbInFile.Name = "txbInFile";
            this.txbInFile.Size = new System.Drawing.Size(433, 20);
            this.txbInFile.TabIndex = 0;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(452, 11);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(105, 23);
            this.btnOpenFile.TabIndex = 1;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // txbOutFile
            // 
            this.txbOutFile.Enabled = false;
            this.txbOutFile.Location = new System.Drawing.Point(12, 130);
            this.txbOutFile.Name = "txbOutFile";
            this.txbOutFile.Size = new System.Drawing.Size(545, 20);
            this.txbOutFile.TabIndex = 4;
            // 
            // btnOutDir
            // 
            this.btnOutDir.Location = new System.Drawing.Point(451, 156);
            this.btnOutDir.Name = "btnOutDir";
            this.btnOutDir.Size = new System.Drawing.Size(106, 23);
            this.btnOutDir.TabIndex = 5;
            this.btnOutDir.Text = "Output";
            this.btnOutDir.UseVisualStyleBackColor = true;
            this.btnOutDir.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // btnGen
            // 
            this.btnGen.Location = new System.Drawing.Point(228, 156);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(127, 23);
            this.btnGen.TabIndex = 6;
            this.btnGen.Text = "Generate";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // btnNew
            // 
            this.btnNew.Enabled = false;
            this.btnNew.Location = new System.Drawing.Point(12, 156);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(98, 23);
            this.btnNew.TabIndex = 9;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxAudioCodec);
            this.groupBox1.Controls.Add(this.cbxVideoCodec);
            this.groupBox1.Controls.Add(this.chkAEnc);
            this.groupBox1.Controls.Add(this.chkVEnc);
            this.groupBox1.Location = new System.Drawing.Point(13, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 85);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Encoders";
            // 
            // cbxAudioCodec
            // 
            this.cbxAudioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAudioCodec.Enabled = false;
            this.cbxAudioCodec.FormattingEnabled = true;
            this.cbxAudioCodec.Location = new System.Drawing.Point(79, 50);
            this.cbxAudioCodec.Name = "cbxAudioCodec";
            this.cbxAudioCodec.Size = new System.Drawing.Size(113, 21);
            this.cbxAudioCodec.TabIndex = 3;
            this.cbxAudioCodec.SelectedIndexChanged += new System.EventHandler(this.cbxAudioCodec_SelectedIndexChanged);
            // 
            // cbxVideoCodec
            // 
            this.cbxVideoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVideoCodec.Enabled = false;
            this.cbxVideoCodec.FormattingEnabled = true;
            this.cbxVideoCodec.Location = new System.Drawing.Point(79, 17);
            this.cbxVideoCodec.Name = "cbxVideoCodec";
            this.cbxVideoCodec.Size = new System.Drawing.Size(113, 21);
            this.cbxVideoCodec.TabIndex = 2;
            // 
            // chkAEnc
            // 
            this.chkAEnc.AutoSize = true;
            this.chkAEnc.Enabled = false;
            this.chkAEnc.Location = new System.Drawing.Point(19, 52);
            this.chkAEnc.Name = "chkAEnc";
            this.chkAEnc.Size = new System.Drawing.Size(53, 17);
            this.chkAEnc.TabIndex = 1;
            this.chkAEnc.Text = "Audio";
            this.chkAEnc.UseVisualStyleBackColor = true;
            this.chkAEnc.CheckedChanged += new System.EventHandler(this.chkAEnc_CheckedChanged);
            // 
            // chkVEnc
            // 
            this.chkVEnc.AutoSize = true;
            this.chkVEnc.Enabled = false;
            this.chkVEnc.Location = new System.Drawing.Point(19, 19);
            this.chkVEnc.Name = "chkVEnc";
            this.chkVEnc.Size = new System.Drawing.Size(53, 17);
            this.chkVEnc.TabIndex = 0;
            this.chkVEnc.Text = "Video";
            this.chkVEnc.UseVisualStyleBackColor = true;
            this.chkVEnc.CheckedChanged += new System.EventHandler(this.chkVEnc_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbMKV);
            this.groupBox2.Controls.Add(this.chbMP4);
            this.groupBox2.Location = new System.Drawing.Point(409, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(148, 84);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output Containers";
            // 
            // chbMKV
            // 
            this.chbMKV.AutoSize = true;
            this.chbMKV.Enabled = false;
            this.chbMKV.Location = new System.Drawing.Point(82, 37);
            this.chbMKV.Name = "chbMKV";
            this.chbMKV.Size = new System.Drawing.Size(49, 17);
            this.chbMKV.TabIndex = 1;
            this.chbMKV.Text = "MKV";
            this.chbMKV.UseVisualStyleBackColor = true;
            this.chbMKV.CheckedChanged += new System.EventHandler(this.chbMKV_CheckedChanged);
            // 
            // chbMP4
            // 
            this.chbMP4.AutoSize = true;
            this.chbMP4.Enabled = false;
            this.chbMP4.Location = new System.Drawing.Point(19, 37);
            this.chbMP4.Name = "chbMP4";
            this.chbMP4.Size = new System.Drawing.Size(48, 17);
            this.chbMP4.TabIndex = 0;
            this.chbMP4.Text = "MP4";
            this.chbMP4.UseVisualStyleBackColor = true;
            this.chbMP4.CheckedChanged += new System.EventHandler(this.chbMP4_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbxChannels);
            this.groupBox3.Controls.Add(this.cbxLanguage);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(222, 39);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(181, 85);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Audio Options";
            // 
            // cbxChannels
            // 
            this.cbxChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxChannels.Enabled = false;
            this.cbxChannels.FormattingEnabled = true;
            this.cbxChannels.Location = new System.Drawing.Point(74, 50);
            this.cbxChannels.Name = "cbxChannels";
            this.cbxChannels.Size = new System.Drawing.Size(101, 21);
            this.cbxChannels.TabIndex = 3;
            // 
            // cbxLanguage
            // 
            this.cbxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLanguage.Enabled = false;
            this.cbxLanguage.FormattingEnabled = true;
            this.cbxLanguage.Location = new System.Drawing.Point(74, 17);
            this.cbxLanguage.Name = "cbxLanguage";
            this.cbxLanguage.Size = new System.Drawing.Size(100, 21);
            this.cbxLanguage.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Channels: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Language: ";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 191);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnGen);
            this.Controls.Add(this.btnOutDir);
            this.Controls.Add(this.txbOutFile);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.txbInFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Simple AVS Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
        private System.Windows.Forms.CheckBox chbMKV;
        private System.Windows.Forms.CheckBox chbMP4;
        private System.Windows.Forms.ComboBox cbxVideoCodec;
        private System.Windows.Forms.CheckBox chkVEnc;
        private System.Windows.Forms.CheckBox chkAEnc;
        private System.Windows.Forms.ComboBox cbxAudioCodec;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbxChannels;
        private System.Windows.Forms.ComboBox cbxLanguage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

