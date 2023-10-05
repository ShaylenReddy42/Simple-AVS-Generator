using SimpleAVSGeneratorCore;
using SimpleAVSGeneratorCore.Support;

using SimpleAVSGeneratorCore.Services;
using static SimpleAVSGeneratorCore.Support.Video;
using static SimpleAVSGeneratorCore.Support.Audio;

using System.Reflection;

namespace SimpleAVSGenerator;

public partial class MainForm : Form
{
    private readonly ILogger<MainForm> logger;
    private readonly IConfiguration configuration;
    private readonly Extensions extensions;
    private readonly IFileWriterService fileWriterService;

    //Variables for dragging the form
    private bool dragging = false;
    private Point dragCursorPoint;
    private Point dragFormPoint;

    private readonly string home = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Temp");
    InputFile? input = null;

    public MainForm(
        ILogger<MainForm> logger,
        IConfiguration configuration,
        Extensions extensions,
        IFileWriterService fileWriterService)
    {
        this.logger = logger;
        this.configuration = configuration;
        this.extensions = extensions;
        this.fileWriterService = fileWriterService;

        // easter egg
        this.logger.LogInformation("Testing configuration: TestConfig is {TestConfig}", this.configuration["TestConfig"]);

        InitializeComponent();

        var informationalVersion = typeof(MainForm).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                                ?? "0.0.0+0-unknown";

        lbltitle.Text += $" v{informationalVersion}";

        if (Properties.Settings.Default.Location.X <= 0 ||
            Properties.Settings.Default.Location.Y <= 0)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }
        else
        {
            DataBindings.Add("Location", Properties.Settings.Default, "Location", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        txbOutFile.Text = home;
        PopulateComboBoxesAsync().GetAwaiter().GetResult();
    }

    #region Methods
    private async Task PopulateComboBoxesAsync()
    {
        cmbVideoCodec.Items.AddRange(await GetOutputVideoCodecsAsync());
        cmbVideoCodec.SelectedIndex = 0;

        cmbAudioCodec.Items.AddRange(await GetOutputAudioCodecsAsync());
        cmbAudioCodec.SelectedIndex = 0;

        cmbKeyframeInterval.Items.AddRange(await GetKeyframeIntervalsAsync());
        cmbKeyframeInterval.SelectedIndex = 0;

        cmbLanguage.Items.AddRange(await GetLanguagesAsync());
        cmbLanguage.SelectedIndex = 0;

        await SetSelectableAudioBitratesAsync();
    }

    private async Task SetSelectableAudioBitratesAsync()
    {
        cmbBitrate.Items.Clear();

        string? audioCodec    = (string)cmbAudioCodec.SelectedItem,
                audioChannels = input?.Audio.SourceChannels ?? "2.0";

        (object[] selectableAudioBitrates, int defaultAudioBitrate) = await GetSelectableAndDefaultAudioBitratesAsync(audioCodec, audioChannels);

        cmbBitrate.Items.AddRange(selectableAudioBitrates);
        cmbBitrate.SelectedItem = defaultAudioBitrate;
    }

    private async Task EnableUIAsync()
    {
        if (input is null)
        {
            return;
        }

        cbxVideo.Enabled = input.FileInfo.HasVideo;
        cbxVideo.Checked = input.FileInfo.HasVideo;
        cbxAudio.Enabled = input.FileInfo.HasAudio;
        cbxAudio.Checked = input.FileInfo.HasAudio;
        cbxMP4.Enabled = input.FileInfo.HasVideo;
        cbxMKV.Enabled = input.FileInfo.HasVideo;

        await SetSelectableAudioBitratesAsync();
    }

    private void New()
    {
        txbInFile.Clear();

        txbOutFile.Text = home;

        cbxVideo.Checked = false;
        cbxVideo.Enabled = false;

        cmbVideoCodec.SelectedIndex = 0;
        cmbKeyframeInterval.SelectedIndex = 0;

        cbxAudio.Enabled = false;
        cbxAudio.Checked = false;

        cmbAudioCodec.SelectedIndex = 0;
        cmbLanguage.SelectedIndex = 0;

        cbxMP4.Enabled = false;
        cbxMP4.Checked = false;

        cbxMKV.Enabled = false;
        cbxMKV.Checked = false;

        btnNew.Enabled = false;
        btnOpenFile.Enabled = true;
        btnOutDir.Enabled = false;

        input = null;
    }
    #endregion Methods

    #region Buttons
    private async void btnOpenFile_Click(object sender, EventArgs e)
    {
        string filterSupportedExts = $"All Supported|{extensions.SupportedContainerExts};{extensions.SupportedVideoExts};{extensions.SupportedAudioExts}",
               filterContainerExts = $"Container Types [{extensions.FilterContainerExts}]|{extensions.SupportedContainerExts}",
               filterVideoExts     = $"Video Types [{extensions.FilterVideoExts}]|{extensions.SupportedVideoExts}",
               filterAudioExts     = $"Audio Types [{extensions.FilterAudioExts}]|{extensions.SupportedAudioExts}";
        
        OpenFileDialog ofd = new()
        {
            Multiselect = false,
            Title = "Open File",
            Filter = $"{filterSupportedExts}|{filterContainerExts}|{filterVideoExts}|{filterAudioExts}"
        };

        input = ofd.ShowDialog() == DialogResult.OK ? new(ofd.FileName, home) : null;

        if (input is not null)
        {
            await EnableUIAsync();

            btnOpenFile.Enabled = false;
            btnNew.Enabled = true;
            btnOutDir.Enabled = true;
        }

        txbInFile.Text = input?.FileInfo.FileName;
        txbOutFile.Text = input is not null ? input.ScriptFile : home;
    }

    private void btnOut_Click(object sender, EventArgs e)
    {
        if (input is null)
        {
            return;
        }

        FolderBrowserDialog fbd = new();
        input.HomeDir = fbd.ShowDialog() is DialogResult.OK ? $@"{fbd.SelectedPath}\" : input.HomeDir;

        txbOutFile.Text = input.ScriptFile;
    }

    private async void btnGen_Click(object sender, EventArgs e)
    {
        if (input is null)
        {
            MessageBox.Show("Please Input A File First");
            return;
        }

        input.Video.Enabled = cbxVideo.Checked;
        input.Video.Codec = (string)cmbVideoCodec.SelectedItem;
        input.Video.KeyframeIntervalInSeconds = (string)cmbKeyframeInterval.SelectedItem;

        input.Audio.Enabled = cbxAudio.Checked;
        input.Audio.Codec = (string)cmbAudioCodec.SelectedItem;
        input.Audio.Bitrate = (int)cmbBitrate.SelectedItem;
        input.Audio.Language = (string)cmbLanguage.SelectedItem;

        input.OutputContainer = cbxMP4.Checked switch
        {
            true => "MP4",
            false => cbxMKV.Checked switch
            {
                true => "MKV",
                false => null
            }
        };

        var scriptsCreated = await input.CreateScriptsAsync(fileWriterService);

        if (scriptsCreated is "")
        {
            MessageBox.Show("No scripts will be created");
        }
        else
        {
            New();
        }
    }

    private void btnNew_Click(object sender, EventArgs e) { New(); }
    #endregion Buttons

    #region ComponentEvents
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
        lbltitle.ForeColor    = Color.FromArgb(200, 200, 200);
        lblMinimize.ForeColor = Color.FromArgb(200, 200, 200);
        lblClose.ForeColor    = Color.FromArgb(200, 200, 200);
    }

    private void MainForm_Activated(object sender, EventArgs e)
    {
        lbltitle.ForeColor    = Color.White;
        lblMinimize.ForeColor = Color.White;
        lblClose.ForeColor    = Color.White;
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        Properties.Settings.Default.Location = Location;
        Properties.Settings.Default.Save();
    }

    private void cbxVideo_CheckedChanged(object sender, EventArgs e)
    {
        cbxMP4.Checked =  cbxVideo.Checked
                      &&  cbxMP4.Enabled
                      && !cbxMKV.Checked;

        cmbVideoCodec.Enabled = cbxVideo.Checked;
        cmbKeyframeInterval.Enabled = cbxVideo.Checked;
    }

    private void cbxAudio_CheckedChanged(object sender, EventArgs e)
    {
        cmbAudioCodec.Enabled = cbxAudio.Checked;

        cmbLanguage.Enabled = cbxAudio.Checked;
        cmbBitrate.Enabled = cbxAudio.Checked;
    }
    
    private void cmbVideoCodec_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((string)cmbVideoCodec.SelectedItem is "Mux Original" && input?.FileInfo.IsSupportedByMP4Box is false)
        {
            cbxMP4.Enabled = false;
            cbxMP4.Checked = false;
            cbxMKV.Checked = true;
        }
        else if (input is not null)
        {
            cbxMP4.Enabled = true;
        }
    }

    private async void cmbAudioCodec_SelectedIndexChanged(object sender, EventArgs e) => 
        await SetSelectableAudioBitratesAsync();

    private void cbxMP4_CheckedChanged(object sender, EventArgs e) =>
        cbxMKV.Checked = cbxMP4.Checked switch
        {
            true  => false,
            false => cbxMKV.Checked
        };

    private void cbxMKV_CheckedChanged(object sender, EventArgs e) =>
        cbxMP4.Checked = cbxMKV.Checked switch
        {
            true  => false,
            false => cbxMP4.Checked
        };

    private void lblClose_MouseEnter(object sender, EventArgs e) =>
        lblClose.BackColor = Color.Red;

    private void lblClose_MouseLeave(object sender, EventArgs e) =>
        lblClose.BackColor = Color.FromArgb(40, 40, 40);

    private void lblClose_Click(object sender, EventArgs e) =>
        Application.Exit();

    private void lblMinimize_MouseEnter(object sender, EventArgs e) =>
        lblMinimize.BackColor = Color.FromArgb(60, 60, 60);

    private void lblMinimize_MouseLeave(object sender, EventArgs e) =>
        lblMinimize.BackColor = Color.FromArgb(40, 40, 40);

    private void lblMinimize_Click(object sender, EventArgs e) =>
        WindowState = FormWindowState.Minimized;

    #endregion ComponentEvents
}
