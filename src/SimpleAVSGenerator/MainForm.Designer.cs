namespace SimpleAVSGenerator;

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
        InputFileTextBox = new TextBox();
        OpenFileButton = new Button();
        OutputFileTextBox = new TextBox();
        OutputDirectoryButton = new Button();
        GenerateButton = new Button();
        NewButton = new Button();
        EncodersGroupBox = new GroupBox();
        AudioCodecComboBox = new ComboBox();
        VideoCodecComboBox = new ComboBox();
        EnableAudioCheckBox = new CheckBox();
        EnableVideoCheckBox = new CheckBox();
        OutputContainersGroupBox = new GroupBox();
        MKVCheckBox = new CheckBox();
        MP4CheckBox = new CheckBox();
        AudioOptionsGroupBox = new GroupBox();
        AudioBitrateComboBox = new ComboBox();
        label3 = new Label();
        AudioLanguageComboBox = new ComboBox();
        label1 = new Label();
        TitleLabel = new Label();
        CloseLabel = new Label();
        MinimizeLabel = new Label();
        VideoOptionsGroupBox = new GroupBox();
        KeyframeIntervalComboBox = new ComboBox();
        label5 = new Label();
        EncodersGroupBox.SuspendLayout();
        OutputContainersGroupBox.SuspendLayout();
        AudioOptionsGroupBox.SuspendLayout();
        VideoOptionsGroupBox.SuspendLayout();
        SuspendLayout();
        // 
        // InputFileTextBox
        // 
        InputFileTextBox.BackColor = Color.FromArgb(50, 50, 50);
        InputFileTextBox.ForeColor = Color.White;
        InputFileTextBox.Location = new Point(14, 44);
        InputFileTextBox.Margin = new Padding(4, 3, 4, 3);
        InputFileTextBox.Name = "InputFileTextBox";
        InputFileTextBox.ReadOnly = true;
        InputFileTextBox.Size = new Size(653, 23);
        InputFileTextBox.TabIndex = 0;
        // 
        // OpenFileButton
        // 
        OpenFileButton.BackColor = Color.RoyalBlue;
        OpenFileButton.FlatStyle = FlatStyle.Flat;
        OpenFileButton.ForeColor = Color.White;
        OpenFileButton.Location = new Point(674, 42);
        OpenFileButton.Margin = new Padding(4, 3, 4, 3);
        OpenFileButton.Name = "OpenFileButton";
        OpenFileButton.Size = new Size(122, 27);
        OpenFileButton.TabIndex = 1;
        OpenFileButton.Text = "Open File";
        OpenFileButton.UseVisualStyleBackColor = false;
        OpenFileButton.Click += OpenFileButton_Click;
        // 
        // OutputFileTextBox
        // 
        OutputFileTextBox.BackColor = Color.FromArgb(50, 50, 50);
        OutputFileTextBox.ForeColor = Color.White;
        OutputFileTextBox.Location = new Point(13, 223);
        OutputFileTextBox.Margin = new Padding(4, 3, 4, 3);
        OutputFileTextBox.Name = "OutputFileTextBox";
        OutputFileTextBox.ReadOnly = true;
        OutputFileTextBox.Size = new Size(783, 23);
        OutputFileTextBox.TabIndex = 4;
        // 
        // OutputDirectoryButton
        // 
        OutputDirectoryButton.BackColor = Color.RoyalBlue;
        OutputDirectoryButton.Enabled = false;
        OutputDirectoryButton.FlatStyle = FlatStyle.Flat;
        OutputDirectoryButton.ForeColor = Color.White;
        OutputDirectoryButton.Location = new Point(673, 253);
        OutputDirectoryButton.Margin = new Padding(4, 3, 4, 3);
        OutputDirectoryButton.Name = "OutputDirectoryButton";
        OutputDirectoryButton.Size = new Size(124, 27);
        OutputDirectoryButton.TabIndex = 5;
        OutputDirectoryButton.Text = "Output";
        OutputDirectoryButton.UseVisualStyleBackColor = false;
        OutputDirectoryButton.Click += OutputDirectory_Click;
        // 
        // GenerateButton
        // 
        GenerateButton.BackColor = Color.RoyalBlue;
        GenerateButton.FlatStyle = FlatStyle.Flat;
        GenerateButton.ForeColor = Color.White;
        GenerateButton.Location = new Point(336, 253);
        GenerateButton.Margin = new Padding(4, 3, 4, 3);
        GenerateButton.Name = "GenerateButton";
        GenerateButton.Size = new Size(148, 27);
        GenerateButton.TabIndex = 6;
        GenerateButton.Text = "Generate";
        GenerateButton.UseVisualStyleBackColor = false;
        GenerateButton.Click += GenerateButton_Click;
        // 
        // NewButton
        // 
        NewButton.BackColor = Color.RoyalBlue;
        NewButton.Enabled = false;
        NewButton.FlatStyle = FlatStyle.Flat;
        NewButton.ForeColor = Color.White;
        NewButton.Location = new Point(13, 253);
        NewButton.Margin = new Padding(4, 3, 4, 3);
        NewButton.Name = "NewButton";
        NewButton.Size = new Size(114, 27);
        NewButton.TabIndex = 9;
        NewButton.Text = "New";
        NewButton.UseVisualStyleBackColor = false;
        NewButton.Click += NewButton_Click;
        // 
        // EncodersGroupBox
        // 
        EncodersGroupBox.BackColor = Color.FromArgb(50, 50, 50);
        EncodersGroupBox.Controls.Add(AudioCodecComboBox);
        EncodersGroupBox.Controls.Add(VideoCodecComboBox);
        EncodersGroupBox.Controls.Add(EnableAudioCheckBox);
        EncodersGroupBox.Controls.Add(EnableVideoCheckBox);
        EncodersGroupBox.ForeColor = Color.White;
        EncodersGroupBox.Location = new Point(14, 75);
        EncodersGroupBox.Margin = new Padding(4, 3, 4, 3);
        EncodersGroupBox.Name = "EncodersGroupBox";
        EncodersGroupBox.Padding = new Padding(4, 3, 4, 3);
        EncodersGroupBox.Size = new Size(180, 142);
        EncodersGroupBox.TabIndex = 12;
        EncodersGroupBox.TabStop = false;
        EncodersGroupBox.Text = "Encoders";
        // 
        // AudioCodecComboBox
        // 
        AudioCodecComboBox.BackColor = Color.FromArgb(60, 60, 60);
        AudioCodecComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        AudioCodecComboBox.Enabled = false;
        AudioCodecComboBox.FlatStyle = FlatStyle.Popup;
        AudioCodecComboBox.ForeColor = Color.White;
        AudioCodecComboBox.FormattingEnabled = true;
        AudioCodecComboBox.ImeMode = ImeMode.NoControl;
        AudioCodecComboBox.Location = new Point(22, 106);
        AudioCodecComboBox.Margin = new Padding(4, 3, 4, 3);
        AudioCodecComboBox.Name = "AudioCodecComboBox";
        AudioCodecComboBox.Size = new Size(131, 23);
        AudioCodecComboBox.TabIndex = 3;
        AudioCodecComboBox.SelectedIndexChanged += AudioCodecComboBox_SelectedIndexChanged;
        // 
        // VideoCodecComboBox
        // 
        VideoCodecComboBox.BackColor = Color.FromArgb(60, 60, 60);
        VideoCodecComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        VideoCodecComboBox.Enabled = false;
        VideoCodecComboBox.FlatStyle = FlatStyle.Popup;
        VideoCodecComboBox.ForeColor = Color.White;
        VideoCodecComboBox.FormattingEnabled = true;
        VideoCodecComboBox.ImeMode = ImeMode.NoControl;
        VideoCodecComboBox.Location = new Point(22, 48);
        VideoCodecComboBox.Margin = new Padding(4, 3, 4, 3);
        VideoCodecComboBox.Name = "VideoCodecComboBox";
        VideoCodecComboBox.Size = new Size(131, 23);
        VideoCodecComboBox.TabIndex = 2;
        VideoCodecComboBox.SelectedIndexChanged += VideoCodecComboBox_SelectedIndexChanged;
        // 
        // EnableAudioCheckBox
        // 
        EnableAudioCheckBox.AutoSize = true;
        EnableAudioCheckBox.Enabled = false;
        EnableAudioCheckBox.ForeColor = Color.White;
        EnableAudioCheckBox.Location = new Point(22, 80);
        EnableAudioCheckBox.Margin = new Padding(4, 3, 4, 3);
        EnableAudioCheckBox.Name = "EnableAudioCheckBox";
        EnableAudioCheckBox.Size = new Size(58, 19);
        EnableAudioCheckBox.TabIndex = 1;
        EnableAudioCheckBox.Text = "Audio";
        EnableAudioCheckBox.UseVisualStyleBackColor = true;
        EnableAudioCheckBox.CheckedChanged += AudioEnabledCheckBox_CheckedChanged;
        // 
        // EnableVideoCheckBox
        // 
        EnableVideoCheckBox.AutoSize = true;
        EnableVideoCheckBox.Enabled = false;
        EnableVideoCheckBox.ForeColor = Color.White;
        EnableVideoCheckBox.Location = new Point(22, 22);
        EnableVideoCheckBox.Margin = new Padding(4, 3, 4, 3);
        EnableVideoCheckBox.Name = "EnableVideoCheckBox";
        EnableVideoCheckBox.Size = new Size(56, 19);
        EnableVideoCheckBox.TabIndex = 0;
        EnableVideoCheckBox.Text = "Video";
        EnableVideoCheckBox.UseVisualStyleBackColor = true;
        EnableVideoCheckBox.CheckedChanged += VideoEnabledCheckBox_CheckedChanged;
        // 
        // OutputContainersGroupBox
        // 
        OutputContainersGroupBox.BackColor = Color.FromArgb(50, 50, 50);
        OutputContainersGroupBox.Controls.Add(MKVCheckBox);
        OutputContainersGroupBox.Controls.Add(MP4CheckBox);
        OutputContainersGroupBox.ForeColor = Color.White;
        OutputContainersGroupBox.Location = new Point(624, 76);
        OutputContainersGroupBox.Margin = new Padding(4, 3, 4, 3);
        OutputContainersGroupBox.Name = "OutputContainersGroupBox";
        OutputContainersGroupBox.Padding = new Padding(4, 3, 4, 3);
        OutputContainersGroupBox.Size = new Size(173, 141);
        OutputContainersGroupBox.TabIndex = 13;
        OutputContainersGroupBox.TabStop = false;
        OutputContainersGroupBox.Text = "Output Containers";
        // 
        // MKVCheckBox
        // 
        MKVCheckBox.AutoSize = true;
        MKVCheckBox.Enabled = false;
        MKVCheckBox.ForeColor = Color.White;
        MKVCheckBox.Location = new Point(58, 84);
        MKVCheckBox.Margin = new Padding(4, 3, 4, 3);
        MKVCheckBox.Name = "MKVCheckBox";
        MKVCheckBox.Size = new Size(51, 19);
        MKVCheckBox.TabIndex = 1;
        MKVCheckBox.Text = "MKV";
        MKVCheckBox.UseVisualStyleBackColor = true;
        MKVCheckBox.CheckedChanged += MKVCheckBox_CheckedChanged;
        // 
        // MP4CheckBox
        // 
        MP4CheckBox.AutoSize = true;
        MP4CheckBox.Enabled = false;
        MP4CheckBox.ForeColor = Color.White;
        MP4CheckBox.Location = new Point(58, 43);
        MP4CheckBox.Margin = new Padding(4, 3, 4, 3);
        MP4CheckBox.Name = "MP4CheckBox";
        MP4CheckBox.Size = new Size(50, 19);
        MP4CheckBox.TabIndex = 0;
        MP4CheckBox.Text = "MP4";
        MP4CheckBox.UseVisualStyleBackColor = true;
        MP4CheckBox.CheckedChanged += MP4CheckBox_CheckedChanged;
        // 
        // AudioOptionsGroupBox
        // 
        AudioOptionsGroupBox.BackColor = Color.FromArgb(50, 50, 50);
        AudioOptionsGroupBox.Controls.Add(AudioBitrateComboBox);
        AudioOptionsGroupBox.Controls.Add(label3);
        AudioOptionsGroupBox.Controls.Add(AudioLanguageComboBox);
        AudioOptionsGroupBox.Controls.Add(label1);
        AudioOptionsGroupBox.ForeColor = Color.White;
        AudioOptionsGroupBox.Location = new Point(406, 76);
        AudioOptionsGroupBox.Margin = new Padding(4, 3, 4, 3);
        AudioOptionsGroupBox.Name = "AudioOptionsGroupBox";
        AudioOptionsGroupBox.Padding = new Padding(4, 3, 4, 3);
        AudioOptionsGroupBox.Size = new Size(211, 140);
        AudioOptionsGroupBox.TabIndex = 14;
        AudioOptionsGroupBox.TabStop = false;
        AudioOptionsGroupBox.Text = "Audio Options";
        // 
        // AudioBitrateComboBox
        // 
        AudioBitrateComboBox.BackColor = Color.FromArgb(60, 60, 60);
        AudioBitrateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        AudioBitrateComboBox.Enabled = false;
        AudioBitrateComboBox.FlatStyle = FlatStyle.Popup;
        AudioBitrateComboBox.ForeColor = Color.White;
        AudioBitrateComboBox.FormattingEnabled = true;
        AudioBitrateComboBox.ImeMode = ImeMode.NoControl;
        AudioBitrateComboBox.Location = new Point(24, 105);
        AudioBitrateComboBox.Margin = new Padding(4, 3, 4, 3);
        AudioBitrateComboBox.Name = "AudioBitrateComboBox";
        AudioBitrateComboBox.Size = new Size(155, 23);
        AudioBitrateComboBox.TabIndex = 5;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.ForeColor = Color.White;
        label3.Location = new Point(24, 80);
        label3.Margin = new Padding(4, 0, 4, 0);
        label3.Name = "label3";
        label3.Size = new Size(44, 15);
        label3.TabIndex = 4;
        label3.Text = "Bitrate:";
        // 
        // AudioLanguageComboBox
        // 
        AudioLanguageComboBox.BackColor = Color.FromArgb(60, 60, 60);
        AudioLanguageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        AudioLanguageComboBox.Enabled = false;
        AudioLanguageComboBox.FlatStyle = FlatStyle.Popup;
        AudioLanguageComboBox.ForeColor = Color.White;
        AudioLanguageComboBox.FormattingEnabled = true;
        AudioLanguageComboBox.ImeMode = ImeMode.NoControl;
        AudioLanguageComboBox.Location = new Point(24, 47);
        AudioLanguageComboBox.Margin = new Padding(4, 3, 4, 3);
        AudioLanguageComboBox.Name = "AudioLanguageComboBox";
        AudioLanguageComboBox.Size = new Size(155, 23);
        AudioLanguageComboBox.TabIndex = 2;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.ForeColor = Color.White;
        label1.Location = new Point(24, 22);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(65, 15);
        label1.TabIndex = 0;
        label1.Text = "Language: ";
        // 
        // TitleLabel
        // 
        TitleLabel.AutoSize = true;
        TitleLabel.Font = new Font("Calibri", 10F, FontStyle.Bold, GraphicsUnit.Point);
        TitleLabel.ForeColor = Color.White;
        TitleLabel.Location = new Point(14, 10);
        TitleLabel.Margin = new Padding(4, 0, 4, 0);
        TitleLabel.Name = "TitleLabel";
        TitleLabel.Size = new Size(136, 17);
        TitleLabel.TabIndex = 15;
        TitleLabel.Text = "Simple AVS Generator";
        // 
        // CloseLabel
        // 
        CloseLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
        CloseLabel.ForeColor = Color.White;
        CloseLabel.Location = new Point(768, 5);
        CloseLabel.Margin = new Padding(6);
        CloseLabel.Name = "CloseLabel";
        CloseLabel.Size = new Size(29, 29);
        CloseLabel.TabIndex = 16;
        CloseLabel.Text = "X";
        CloseLabel.TextAlign = ContentAlignment.MiddleCenter;
        CloseLabel.Click += CloseLabel_Click;
        CloseLabel.MouseEnter += CloseLabel_MouseEnter;
        CloseLabel.MouseLeave += CloseLabel_MouseLeave;
        // 
        // MinimizeLabel
        // 
        MinimizeLabel.ForeColor = Color.White;
        MinimizeLabel.Location = new Point(729, 5);
        MinimizeLabel.Margin = new Padding(4, 0, 4, 0);
        MinimizeLabel.Name = "MinimizeLabel";
        MinimizeLabel.Size = new Size(29, 29);
        MinimizeLabel.TabIndex = 17;
        MinimizeLabel.Text = "_";
        MinimizeLabel.TextAlign = ContentAlignment.MiddleCenter;
        MinimizeLabel.Click += MinimizeLabel_Click;
        MinimizeLabel.MouseEnter += MinimizeLabel_MouseEnter;
        MinimizeLabel.MouseLeave += MinimizeLabel_MouseLeave;
        // 
        // VideoOptionsGroupBox
        // 
        VideoOptionsGroupBox.BackColor = Color.FromArgb(50, 50, 50);
        VideoOptionsGroupBox.Controls.Add(KeyframeIntervalComboBox);
        VideoOptionsGroupBox.Controls.Add(label5);
        VideoOptionsGroupBox.ForeColor = Color.White;
        VideoOptionsGroupBox.Location = new Point(202, 76);
        VideoOptionsGroupBox.Margin = new Padding(4, 3, 4, 3);
        VideoOptionsGroupBox.Name = "VideoOptionsGroupBox";
        VideoOptionsGroupBox.Padding = new Padding(4, 3, 4, 3);
        VideoOptionsGroupBox.Size = new Size(197, 140);
        VideoOptionsGroupBox.TabIndex = 18;
        VideoOptionsGroupBox.TabStop = false;
        VideoOptionsGroupBox.Text = "Video Options";
        // 
        // KeyframeIntervalComboBox
        // 
        KeyframeIntervalComboBox.BackColor = Color.FromArgb(60, 60, 60);
        KeyframeIntervalComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        KeyframeIntervalComboBox.Enabled = false;
        KeyframeIntervalComboBox.FlatStyle = FlatStyle.Popup;
        KeyframeIntervalComboBox.ForeColor = Color.White;
        KeyframeIntervalComboBox.FormattingEnabled = true;
        KeyframeIntervalComboBox.Location = new Point(28, 75);
        KeyframeIntervalComboBox.Margin = new Padding(4, 3, 4, 3);
        KeyframeIntervalComboBox.Name = "KeyframeIntervalComboBox";
        KeyframeIntervalComboBox.Size = new Size(140, 23);
        KeyframeIntervalComboBox.TabIndex = 3;
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new Point(24, 55);
        label5.Margin = new Padding(4, 0, 4, 0);
        label5.Name = "label5";
        label5.Size = new Size(136, 15);
        label5.TabIndex = 2;
        label5.Text = "Select Keyframe Interval:";
        // 
        // MainForm
        // 
        AllowDrop = true;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(40, 40, 40);
        ClientSize = new Size(813, 294);
        Controls.Add(VideoOptionsGroupBox);
        Controls.Add(MinimizeLabel);
        Controls.Add(CloseLabel);
        Controls.Add(TitleLabel);
        Controls.Add(AudioOptionsGroupBox);
        Controls.Add(OutputContainersGroupBox);
        Controls.Add(EncodersGroupBox);
        Controls.Add(NewButton);
        Controls.Add(GenerateButton);
        Controls.Add(OutputDirectoryButton);
        Controls.Add(OutputFileTextBox);
        Controls.Add(OpenFileButton);
        Controls.Add(InputFileTextBox);
        FormBorderStyle = FormBorderStyle.None;
        Margin = new Padding(4, 3, 4, 3);
        MaximizeBox = false;
        Name = "MainForm";
        Text = "Simple AVS Generator";
        Activated += MainForm_Activated;
        Deactivate += MainForm_Deactivate;
        FormClosing += MainForm_FormClosing;
        MouseDown += MainForm_MouseDown;
        MouseMove += MainForm_MouseMove;
        MouseUp += MainForm_MouseUp;
        EncodersGroupBox.ResumeLayout(false);
        EncodersGroupBox.PerformLayout();
        OutputContainersGroupBox.ResumeLayout(false);
        OutputContainersGroupBox.PerformLayout();
        AudioOptionsGroupBox.ResumeLayout(false);
        AudioOptionsGroupBox.PerformLayout();
        VideoOptionsGroupBox.ResumeLayout(false);
        VideoOptionsGroupBox.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox InputFileTextBox;
    private Button OpenFileButton;
    private TextBox OutputFileTextBox;
    private Button OutputDirectoryButton;
    private Button GenerateButton;
    private Button NewButton;
    private GroupBox EncodersGroupBox;
    private GroupBox OutputContainersGroupBox;
    private CheckBox MKVCheckBox;
    private CheckBox MP4CheckBox;
    private ComboBox VideoCodecComboBox;
    private CheckBox EnableVideoCheckBox;
    private CheckBox EnableAudioCheckBox;
    private ComboBox AudioCodecComboBox;
    private GroupBox AudioOptionsGroupBox;
    private ComboBox AudioLanguageComboBox;
    private Label label1;
    private Label label3;
    private ComboBox AudioBitrateComboBox;
    private Label TitleLabel;
    private Label CloseLabel;
    private Label MinimizeLabel;
    private GroupBox VideoOptionsGroupBox;
    private ComboBox KeyframeIntervalComboBox;
    private Label label5;
}

