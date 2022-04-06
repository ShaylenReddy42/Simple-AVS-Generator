/******************************************************************************
 * Copyright (C) 2018-2022 Shaylen Reddy
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along
 * with this program; if not, write to the Free Software Foundation, Inc.,
 * 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
 ******************************************************************************/

using System.Diagnostics;

using SimpleAVSGeneratorCore;
using SimpleAVSGeneratorCore.Support;

using static SimpleAVSGeneratorCore.Support.Video;
using static SimpleAVSGeneratorCore.Support.Audio;

namespace SimpleAVSGenerator;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        lbltitle.Text += $" v{FileVersionInfo.GetVersionInfo(Application.ExecutablePath).ProductVersion}";

        if (Properties.Settings.Default.Location.X <= 0 ||
            Properties.Settings.Default.Location.Y <= 0)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        else
        {
            this.DataBindings.Add("Location", Properties.Settings.Default, "Location", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        txbOutFile.Text = home;
        PopulateComboBoxes();
    }

    //Variables for dragging the form
    private bool dragging = false;
    private Point dragCursorPoint;
    private Point dragFormPoint;

    static string home = $@"C:\Users\{Environment.UserName}\Desktop\Temp\";

    Extensions supportedExts = new();
    InputFile? input = null;

    #region Methods
    void PopulateComboBoxes()
    {
        cmbVideoCodec.Items.AddRange(GetOutputVideoCodecs());
        cmbVideoCodec.SelectedIndex = 0;

        cmbAudioCodec.Items.AddRange(GetOutputAudioCodecs());
        cmbAudioCodec.SelectedIndex = 0;

        cmbKeyframeInterval.Items.AddRange(GetKeyframeIntervals());
        cmbKeyframeInterval.SelectedIndex = 0;

        cmbLanguage.Items.AddRange(GetLanguages());
        cmbLanguage.SelectedIndex = 0;

        SetSelectableAudioBitrates();
    }

    void SetSelectableAudioBitrates()
    {
        cmbBitrate.Items.Clear();

        string? audioCodec    = (string)cmbAudioCodec.SelectedItem,
                audioChannels = input?.Audio.SourceChannels ?? "2.0";

        (object[] selectableAudioBitrates, int defaultAudioBitrate) = GetSelectableAndDefaultAudioBitrates(audioCodec, audioChannels);

        cmbBitrate.Items.AddRange(selectableAudioBitrates);
        cmbBitrate.SelectedItem = defaultAudioBitrate;
    }

    void EnableUI()
    {
        string? type = input?.FileInfo.FileType;
        
        cbxVideo.Enabled = type is not "AUDIO";
        cbxVideo.Checked = type is "VIDEO";
        cbxAudio.Enabled = type is not "VIDEO";
        cbxAudio.Checked = type is "AUDIO";
        cbxMP4.Enabled   = type is not "AUDIO";
        cbxMKV.Enabled   = type is not "AUDIO";

        SetSelectableAudioBitrates();
    }

    void New()
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
    private void btnOpenFile_Click(object sender, EventArgs e)
    {
        string filterSupportedExts = $"All Supported|{supportedExts.SupportedContainerExts};{supportedExts.SupportedVideoExts};{supportedExts.SupportedAudioExts}",
               filterContainerExts = $"Container Types [{supportedExts.FilterContainerExts}]|{supportedExts.SupportedContainerExts}",
               filterVideoExts     = $"Video Types [{supportedExts.FilterVideoExts}]|{supportedExts.SupportedVideoExts}",
               filterAudioExts     = $"Audio Types [{supportedExts.FilterAudioExts}]|{supportedExts.SupportedAudioExts}";
        
        OpenFileDialog ofd = new()
        {
            Multiselect = false,
            Title = "Open File",
            Filter = $"{filterSupportedExts}|{filterContainerExts}|{filterVideoExts}|{filterAudioExts}"
        };

        input = ofd.ShowDialog() == DialogResult.OK ? new(ofd.FileName, home) : null;

        if (input is not null)
        {
            EnableUI();

            btnOpenFile.Enabled = false;
            btnNew.Enabled = true;
            btnOutDir.Enabled = true;
        }

        txbInFile.Text = input?.FileInfo.FileName;
        txbOutFile.Text = input is not null ? input.ScriptFile : home;
    }

    private void btnOut_Click(object sender, EventArgs e)
    {
        if (input is not null)
        {
            FolderBrowserDialog fbd = new();
            input.HomeDir = fbd.ShowDialog() == DialogResult.OK ? $@"{fbd.SelectedPath}\" : input.HomeDir;

            txbOutFile.Text = input?.ScriptFile;
        }
    }

    private void btnGen_Click(object sender, EventArgs e)
    {
        if (input is not null)
        {
            input.Video.Enabled = cbxVideo.Checked;
            input.Video.Codec = (string)cmbVideoCodec.SelectedItem;
            input.Video.KeyframeIntervalInSeconds = (string)cmbKeyframeInterval.SelectedItem;
            
            input.Audio.Enabled = cbxAudio.Checked;
            input.Audio.Codec = (string)cmbAudioCodec.SelectedItem;
            input.Audio.Bitrate = (int)cmbBitrate.SelectedItem;
            input.Audio.Language = (string)cmbLanguage.SelectedItem;
            
            input.OutputContainer = cbxMP4.Checked ? "MP4"
                                         : cbxMKV.Checked ? "MKV"
                                         : null;

            input.CreateScripts(out string scriptsCreated);

            if (scriptsCreated is "")
            {
                MessageBox.Show("No scripts will be created");
            }
            else
            {
                New();
            }
        }
        else MessageBox.Show("Please Input A File First");
    }

    private void btnNew_Click(object sender, EventArgs e) { New(); }
    #endregion Buttons

    #region ComponentEvents
    private void MainForm_MouseDown(object sender, MouseEventArgs e)
    {
        dragging = true;
        dragCursorPoint = Cursor.Position;
        dragFormPoint = this.Location;
    }

    private void MainForm_MouseMove(object sender, MouseEventArgs e)
    {
        if (dragging is true)
        {
            Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            this.Location = Point.Add(dragFormPoint, new Size(dif));
        }
    }

    private void MainForm_MouseUp(object sender, MouseEventArgs e)
    {
        dragging = false;
    }

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

    private void cmbAudioCodec_SelectedIndexChanged(object sender, EventArgs e) { SetSelectableAudioBitrates(); }

    private void cbxMP4_CheckedChanged(object sender, EventArgs e)
    {
        if (cbxMP4.Checked is true)
        {
            cbxMKV.Checked = false;
        }
    }

    private void cbxMKV_CheckedChanged(object sender, EventArgs e)
    {
        if (cbxMKV.Checked is true)
        {
            cbxMP4.Checked = false;
        }
    }

    private void lblClose_MouseEnter(object sender, EventArgs e)
    {
        lblClose.BackColor = Color.Red;
    }

    private void lblClose_MouseLeave(object sender, EventArgs e)
    {
        lblClose.BackColor = Color.FromArgb(40, 40, 40);
    }

    private void lblClose_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void lblMinimize_MouseEnter(object sender, EventArgs e)
    {
        lblMinimize.BackColor = Color.FromArgb(60, 60, 60);
    }

    private void lblMinimize_MouseLeave(object sender, EventArgs e)
    {
        lblMinimize.BackColor = Color.FromArgb(40, 40, 40);
    }

    private void lblMinimize_Click(object sender, EventArgs e)
    {
        this.WindowState = FormWindowState.Minimized;
    }
    #endregion ComponentEvents
}
