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

using Simple_AVS_Generator.Core;
using Simple_AVS_Generator.Core.Support;

using static Simple_AVS_Generator.Core.Enums;
using static Simple_AVS_Generator.Core.Support.Video;
using static Simple_AVS_Generator.Core.Support.Audio;

namespace Simple_AVS_Generator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            lbltitle.Text += $" v{FileVersionInfo.GetVersionInfo(Application.ExecutablePath).ProductVersion}";

            if (Properties.Settings.Default.Location.X <= 0 ||
                Properties.Settings.Default.Location.Y <= 0)
                this.StartPosition = FormStartPosition.CenterScreen;
            else
                this.DataBindings.Add("Location", Properties.Settings.Default, "Location", true, DataSourceUpdateMode.OnPropertyChanged);

            txbOutFile.Text = home;
            PopulateComboBoxes();
        }

        //Variables for dragging the form
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        static string home = $@"C:\Users\{Environment.UserName}\Desktop\Temp\";

        Extensions supportedExts = new();
        InputFileHandler? input = null;

        #region Methods
        void PopulateComboBoxes()
        {
            for (int i = 0; i < outputAudioCodecs.GetLength(0); i++)
                cmbAudioCodec.Items.Add(outputAudioCodecs[i, 1]);

            cmbAudioCodec.SelectedIndex = 0;

            for (int i = 0; i < outputVideoCodecs.GetLength(0); i++)
                cmbVideoCodec.Items.Add(outputVideoCodecs[i, 1]);

            cmbVideoCodec.SelectedIndex = 0;

            for (int i = 0; i < languages.GetLength(0); i++)
                cmbLanguage.Items.Add(languages[i, 1]);

            cmbLanguage.SelectedIndex = 0;
            
            for (int i = 0; i < sourceFPS.GetLength(0); i++)
                cmbSourceFPS.Items.Add(sourceFPS[i, 1]);
            
            cmbSourceFPS.SelectedIndex = 0;

            for (int i = 0; i < keyframeInterval.GetLength(0); i++)
                cmbKeyframeInterval.Items.Add(keyframeInterval[i, 1]);
            
            cmbKeyframeInterval.SelectedIndex = 0;

            cmbChannels.Items.AddRange(outputAudioChannels);
            cmbChannels.SelectedIndex = 0;

            SetSelectableAudioBitrates();
        }

        void SetSelectableAudioBitrates()
        {
            cmbBitrate.Items.Clear();

            int audioCodec    = cmbAudioCodec.SelectedIndex,
                audioChannels = cmbChannels.SelectedIndex == -1 ? 0 : cmbChannels.SelectedIndex;

            for (int i = 0; i < selectableAudioBitrates.GetLength(2); i++)
                cmbBitrate.Items.Add(selectableAudioBitrates[audioCodec, audioChannels, i]);

            cmbBitrate.SelectedItem = defaultAudioBitrates[audioCodec, audioChannels];
        }

        void EnableEncodeAndContainer()
        {
            int? type = input?.common.FileType;
            
            cbxVideo.Enabled = type != (int)ExtensionTypes.AUDIO;
            cbxVideo.Checked = type == (int)ExtensionTypes.VIDEO;
            cbxAudio.Enabled = type != (int)ExtensionTypes.VIDEO;
            cbxAudio.Checked = type == (int)ExtensionTypes.AUDIO;
            cbxMP4.Enabled   = type != (int)ExtensionTypes.AUDIO;
            cbxMKV.Enabled   = type != (int)ExtensionTypes.AUDIO;
        }

        void New()
        {
            txbInFile.Clear();

            txbOutFile.Text = home;

            cbxVideo.Checked = false;
            cbxVideo.Enabled = false;

            cmbVideoCodec.SelectedIndex = 0;
            cmbSourceFPS.SelectedIndex = 0;
            cmbKeyframeInterval.SelectedIndex = 0;

            cbxAudio.Enabled = false;
            cbxAudio.Checked = false;

            cmbAudioCodec.SelectedIndex = 0;
            cmbLanguage.SelectedIndex = 0;
            cmbChannels.SelectedIndex = 0;

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
            
            OpenFileDialog ofd = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open File",
                Filter = $"{filterSupportedExts}|{filterContainerExts}|{filterVideoExts}|{filterAudioExts}"
            };

            input = ofd.ShowDialog() == DialogResult.OK ? new(ofd.FileName) : null;

            if (input is not null)
            {
                input.common.OutputDir = $@"{home}{input.common.FileNameOnly}\";
                EnableEncodeAndContainer();

                btnOpenFile.Enabled = false;
                btnNew.Enabled = true;
                btnOutDir.Enabled = true;
            }

            txbInFile.Text = input?.common.FileName;
            txbOutFile.Text = input is not null ? input.common.ScriptFile : home;
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            if (input is not null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                input.common.OutputDir = fbd.ShowDialog() == DialogResult.OK ? $@"{fbd.SelectedPath}\{input?.common.FileNameOnly}\" : input.common.OutputDir;

                txbOutFile.Text = input?.common.ScriptFile;
            }
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            if (input is not null)
            {
                Directory.CreateDirectory(input.common.OutputDir);

                input.common.Video = cbxVideo.Checked;
                input.common.MuxOriginalVideo = cmbVideoCodec.SelectedIndex == (int)VideoCodecs.Original;
                input.common.VideoCodec = cmbVideoCodec.SelectedIndex;
                input.common.SourceFPS = (int)sourceFPS[cmbSourceFPS.SelectedIndex, 0];
                input.common.KeyframeIntervalInSeconds = (int)keyframeInterval[cmbKeyframeInterval.SelectedIndex, 0];
                input.common.NeedsToBeResized = cmbVideoCodec.SelectedIndex == (int)VideoCodecs.WhatsApp;
                input.common.VideoExtention = outputVideoCodecs[cmbVideoCodec.SelectedIndex, 0];
                
                input.common.Audio = cbxAudio.Checked;
                input.common.AudioCodec = cmbAudioCodec.SelectedIndex;
                input.common.AudioBitrate = (int)cmbBitrate.SelectedItem;
                input.common.AudioLanguage = languages[cmbLanguage.SelectedIndex, 0];
                input.common.AudioExtension = outputAudioCodecs[cmbAudioCodec.SelectedIndex, 0];
                
                input.common.OutputContainer = cbxMP4.Checked ? (int)OutputContainers.MP4
                                             : cbxMKV.Checked ? (int)OutputContainers.MKV
                                             : null;

                input.CreateScripts();

                New();
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
            if (dragging)
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
            cmbSourceFPS.Enabled = cbxVideo.Checked;
            cmbKeyframeInterval.Enabled = cbxVideo.Checked;
        }

        private void cbxAudio_CheckedChanged(object sender, EventArgs e)
        {
            cmbAudioCodec.Enabled = cbxAudio.Checked;

            cmbLanguage.Enabled = cbxAudio.Checked;
            cmbChannels.Enabled = cbxAudio.Checked;
            cmbBitrate.Enabled = cbxAudio.Checked;
        }
        
        private void cmbVideoCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVideoCodec.SelectedIndex == (int) VideoCodecs.Original && input?.common.IsSupportedByMP4Box is false)
            {
                cbxMP4.Enabled = false;
                cbxMP4.Checked = false;
                cbxMKV.Checked = true;
            }
            else if (input is not null)
                cbxMP4.Enabled = true;
        }

        private void cmbAudioCodec_SelectedIndexChanged(object sender, EventArgs e) { SetSelectableAudioBitrates(); }

        private void cmbChannels_SelectedIndexChanged(object sender, EventArgs e) { SetSelectableAudioBitrates(); }

        private void cbxMP4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxMP4.Checked)
                cbxMKV.Checked = false;
        }

        private void cbxMKV_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxMKV.Checked)
                cbxMP4.Checked = false;
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
}
