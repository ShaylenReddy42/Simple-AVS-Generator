using SimpleAVSGeneratorCore;
using SimpleAVSGeneratorCore.Support;

using SimpleAVSGeneratorCore.Services;
using static SimpleAVSGeneratorCore.Support.Video;
using static SimpleAVSGeneratorCore.Support.Audio;

using System.Reflection;

namespace SimpleAVSGenerator;

public partial class MainForm : Form
{
    private readonly Extensions extensions;
    private readonly ILogger<MainForm> logger;
    private readonly IConfiguration configuration;
    private readonly IInputFileHandlerService inputFileHandlerService;

    //Variables for dragging the form
    private bool dragging = false;
    private Point dragCursorPoint;
    private Point dragFormPoint;

    private readonly string home = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Temp");
    private InputFile? input = null;

    public MainForm(
        Extensions extensions,
        ILogger<MainForm> logger,
        IConfiguration configuration,
        IInputFileHandlerService inputFileHandlerService)
    {
        this.extensions = extensions;
        this.logger = logger;
        this.configuration = configuration;
        this.inputFileHandlerService = inputFileHandlerService;

        InitializeComponent();
    }

    #region Methods
    private async Task PopulateComboBoxesAsync()
    {
        VideoCodecComboBox.Items.AddRange(await GetOutputVideoCodecsAsync());
        VideoCodecComboBox.SelectedIndex = 0;

        AudioCodecComboBox.Items.AddRange(await GetOutputAudioCodecsAsync());
        AudioCodecComboBox.SelectedIndex = 0;

        KeyframeIntervalComboBox.Items.AddRange(await GetKeyframeIntervalsAsync());
        KeyframeIntervalComboBox.SelectedIndex = 0;

        AudioLanguageComboBox.Items.AddRange(await GetLanguagesAsync());
        AudioLanguageComboBox.SelectedIndex = 0;

        await SetSelectableAudioBitratesAsync();
    }

    private async Task SetSelectableAudioBitratesAsync()
    {
        AudioBitrateComboBox.Items.Clear();

        string? audioCodec    = (string)AudioCodecComboBox.SelectedItem,
                audioChannels = input?.Audio.SourceChannels ?? "2.0";

        (object[] selectableAudioBitrates, int defaultAudioBitrate) = await GetSelectableAndDefaultAudioBitratesAsync(audioCodec, audioChannels);

        AudioBitrateComboBox.Items.AddRange(selectableAudioBitrates);
        AudioBitrateComboBox.SelectedItem = defaultAudioBitrate;
    }

    private async Task EnableUIAsync()
    {
        if (input is null)
        {
            return;
        }

        EnableVideoCheckBox.Enabled = input.FileInfo.HasVideo;
        EnableVideoCheckBox.Checked = input.FileInfo.HasVideo;

        EnableAudioCheckBox.Enabled = input.FileInfo.HasAudio;
        EnableAudioCheckBox.Checked = input.FileInfo.HasAudio;

        MP4CheckBox.Enabled = input.FileInfo.HasVideo;
        MKVCheckBox.Enabled = input.FileInfo.HasVideo;

        MP4CheckBox.Checked = input.FileInfo.HasVideo;

        await SetSelectableAudioBitratesAsync();
    }

    private void New()
    {
        InputFileTextBox.Clear();

        OutputFileTextBox.Text = home;

        EnableVideoCheckBox.Checked = false;
        EnableVideoCheckBox.Enabled = false;

        VideoCodecComboBox.SelectedIndex = 0;
        KeyframeIntervalComboBox.SelectedIndex = 0;

        EnableAudioCheckBox.Enabled = false;
        EnableAudioCheckBox.Checked = false;

        AudioCodecComboBox.SelectedIndex = 0;
        AudioLanguageComboBox.SelectedIndex = 0;

        MP4CheckBox.Enabled = false;
        MP4CheckBox.Checked = false;

        MKVCheckBox.Enabled = false;
        MKVCheckBox.Checked = false;

        NewButton.Enabled = false;
        OpenFileButton.Enabled = true;
        OutputDirectoryButton.Enabled = false;

        input = null;
    }
    #endregion Methods

    #region Buttons
    private async void OpenFileButton_Click(object sender, EventArgs e)
    {
        string filterSupportedExts = $"All Supported|{extensions.SupportedContainerExts};{extensions.SupportedVideoExts};{extensions.SupportedAudioExts}",
               filterContainerExts = $"Container Types [{extensions.FilterContainerExts}]|{extensions.SupportedContainerExts}",
               filterVideoExts = $"Video Types [{extensions.FilterVideoExts}]|{extensions.SupportedVideoExts}",
               filterAudioExts = $"Audio Types [{extensions.FilterAudioExts}]|{extensions.SupportedAudioExts}";

        var openFileDialog = new OpenFileDialog()
        {
            Multiselect = false,
            Title = "Open File",
            Filter = $"{filterSupportedExts}|{filterContainerExts}|{filterVideoExts}|{filterAudioExts}"
        };

        input = openFileDialog.ShowDialog() is DialogResult.OK ? await inputFileHandlerService.CreateInputFileAsync(openFileDialog.FileName, home) : null;

        if (input is not null)
        {
            await EnableUIAsync();

            OpenFileButton.Enabled = false;
            NewButton.Enabled = true;
            OutputDirectoryButton.Enabled = true;
        }

        InputFileTextBox.Text = input?.FileInfo.FileName;
        OutputFileTextBox.Text = input is not null ? input.ScriptFile : home;
    }

    private void OutputDirectory_Click(object sender, EventArgs e)
    {
        if (input is null)
        {
            return;
        }

        var folderBrowserDialog = new FolderBrowserDialog();
        input.HomeDir = folderBrowserDialog.ShowDialog() is DialogResult.OK ? folderBrowserDialog.SelectedPath : input.HomeDir;

        OutputFileTextBox.Text = input.ScriptFile;
    }

    private async void GenerateButton_Click(object sender, EventArgs e)
    {
        if (input is null)
        {
            MessageBox.Show("Please Input A File First");
            return;
        }

        input.Video.Enabled = EnableVideoCheckBox.Checked;
        input.Video.Codec = (string)VideoCodecComboBox.SelectedItem;
        input.Video.KeyframeIntervalInSeconds = (string)KeyframeIntervalComboBox.SelectedItem;

        input.Audio.Enabled = EnableAudioCheckBox.Checked;
        input.Audio.Codec = (string)AudioCodecComboBox.SelectedItem;
        input.Audio.Bitrate = (int)AudioBitrateComboBox.SelectedItem;
        input.Audio.Language = (string)AudioLanguageComboBox.SelectedItem;

        input.OutputContainer = MP4CheckBox.Checked switch
        {
            true => "MP4",
            false => MKVCheckBox.Checked switch
            {
                true => "MKV",
                false => null
            }
        };

        var scriptsCreated = await inputFileHandlerService.CreateScriptsAsync(input);

        if (scriptsCreated is "")
        {
            MessageBox.Show("No scripts will be created");
        }
        else
        {
            New();
        }
    }

    private void NewButton_Click(object sender, EventArgs e) { New(); }
    #endregion Buttons

    #region ComponentEvents
    private async void MainForm_Load(object sender, EventArgs e)
    {
        // easter egg
        logger.LogInformation(
            "Testing configuration: TestConfig is {TestConfig}", 
            configuration["TestConfig"]);

        var informationalVersion = typeof(MainForm).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                                ?? "0.0.0+0-unknown";

        TitleLabel.Text += $" v{informationalVersion}";

        if (Properties.Settings.Default.Location.X <= 0 ||
            Properties.Settings.Default.Location.Y <= 0)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }
        else
        {
            DataBindings.Add("Location", Properties.Settings.Default, "Location", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        OutputFileTextBox.Text = home;
        await PopulateComboBoxesAsync();
    }

    private void MainForm_MouseDown(object sender, MouseEventArgs e)
    {
        dragging = true;
        dragCursorPoint = Cursor.Position;
        dragFormPoint = Location;
    }

    private void MainForm_MouseMove(object sender, MouseEventArgs e)
    {
        if (dragging)
        {
            Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            Location = Point.Add(dragFormPoint, new Size(dif));
        }
    }

    private void MainForm_MouseUp(object sender, MouseEventArgs e) =>
        dragging = false;

    private void MainForm_Deactivate(object sender, EventArgs e)
    {
        TitleLabel.ForeColor = Color.FromArgb(200, 200, 200);
        MinimizeLabel.ForeColor = Color.FromArgb(200, 200, 200);
        CloseLabel.ForeColor = Color.FromArgb(200, 200, 200);
    }

    private void MainForm_Activated(object sender, EventArgs e)
    {
        TitleLabel.ForeColor = Color.White;
        MinimizeLabel.ForeColor = Color.White;
        CloseLabel.ForeColor = Color.White;
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        Properties.Settings.Default.Location = Location;
        Properties.Settings.Default.Save();
    }

    private void VideoEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        MP4CheckBox.Checked = EnableVideoCheckBox.Checked
                           && MP4CheckBox.Enabled
                           && !MKVCheckBox.Checked;

        VideoCodecComboBox.Enabled = EnableVideoCheckBox.Checked;
        KeyframeIntervalComboBox.Enabled = EnableVideoCheckBox.Checked;
    }

    private void AudioEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        AudioCodecComboBox.Enabled = EnableAudioCheckBox.Checked;

        AudioLanguageComboBox.Enabled = EnableAudioCheckBox.Checked;
        AudioBitrateComboBox.Enabled = EnableAudioCheckBox.Checked;
    }

    private void VideoCodecComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((string)VideoCodecComboBox.SelectedItem is "Mux Original" && input?.FileInfo.IsSupportedByMP4Box is false)
        {
            MP4CheckBox.Enabled = false;
            MP4CheckBox.Checked = false;
            MKVCheckBox.Checked = true;
        }
        else if (input is not null)
        {
            MP4CheckBox.Enabled = true;
        }
    }

    private async void AudioCodecComboBox_SelectedIndexChanged(object sender, EventArgs e) =>
        await SetSelectableAudioBitratesAsync();

    private void MP4CheckBox_CheckedChanged(object sender, EventArgs e) =>
        MKVCheckBox.Checked = MP4CheckBox.Checked switch
        {
            true => false,
            false => MKVCheckBox.Checked
        };

    private void MKVCheckBox_CheckedChanged(object sender, EventArgs e) =>
        MP4CheckBox.Checked = MKVCheckBox.Checked switch
        {
            true => false,
            false => MP4CheckBox.Checked
        };

    private void CloseLabel_MouseEnter(object sender, EventArgs e) =>
        CloseLabel.BackColor = Color.Red;

    private void CloseLabel_MouseLeave(object sender, EventArgs e) =>
        CloseLabel.BackColor = Color.FromArgb(40, 40, 40);

    private void CloseLabel_Click(object sender, EventArgs e) =>
        Application.Exit();

    private void MinimizeLabel_MouseEnter(object sender, EventArgs e) =>
        MinimizeLabel.BackColor = Color.FromArgb(60, 60, 60);

    private void MinimizeLabel_MouseLeave(object sender, EventArgs e) =>
        MinimizeLabel.BackColor = Color.FromArgb(40, 40, 40);

    private void MinimizeLabel_Click(object sender, EventArgs e) =>
        WindowState = FormWindowState.Minimized;
    #endregion ComponentEvents
}
