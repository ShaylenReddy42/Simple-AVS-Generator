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
using static Simple_AVS_Generator.Core.Enums;

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

        SupportedExts supportedExts = new();
        InputFileHandler? input = null;

        int [] sourceFPS  = { 24, 25, 30, 60 },
               kfInterval = { 2, 5, 10 };

        #region Languages
        string [,] languages =
        {
            //ISO 639-2 language code | Name of language in English
            { "eng", "English"      },
            { "hin", "Hindi"        },
            { "jpn", "Japanese"     },
            { "tam", "Tamil"        },
            { "und", "Undetermined" }
        };
        #endregion Languages

        #region AudioBitrates
        /**
         * A 3D array containing sane audio bitrates
         * for each codec and their channel layouts
         * 
         * 1st Dimension: Codec [AAC-LC, AAC-HE, OPUS]
         * 2nd Dimension: Channels [2, 5.1, 7.1]
         * 3rd Dimension: List of sane bitrates
         */
        int [,,] selectableAudioBitrates =
        {
            //AAC-LC
            {
                {  96, 112, 128, 144, 160, 192 }, //2 Channels
                { 192, 224, 256, 288, 320, 384 }, //5.1 Channels
                { 384, 448, 512, 576, 640, 768 }  //7.1 Channels
            },

            //AAC-HE
            {
                {  32,  40,  48,  56,  64,  80 }, //2 Channels
                {  80,  96, 112, 128, 160, 192 }, //5.1 Channels
                { 112, 128, 160, 192, 224, 256 }  //7.1 Channels
            },

            //OPUS
            {
                {  96, 112, 128, 144, 160, 192 }, //2 Channels
                { 144, 160, 192, 224, 256, 288 }, //5.1 Channels
                { 256, 288, 320, 384, 448, 576 }  //7.1 Channels
            }
        };
        
        /**
         * 1st Dimension: Codec [AAC-LC, AAC-HE, OPUS]
         * 2nd Dimension: Channels [2, 5.1, 7.1]
         */
        int [,] defaultAudioBitrates =
        {
            { 128, 384, 512 }, //AAC-LC
            {  80, 192, 256 }, //AAC-HE
            {  96, 288, 384 }  //OPUS
        };
        #endregion AudioBitrates

        #region Methods
        void PopulateComboBoxes()
        {
            cmbAudioCodec.Items.AddRange
            (
                new string []
                {
                    "AAC-LC",
                    "AAC-HE",
                    "OPUS"
                }
            );
            cmbAudioCodec.SelectedIndex = 0;

            cmbVideoCodec.Items.AddRange
            (
                new string []
                {
                    "HEVC",
                    "AV1",
                    "AVC",
                    "WhatsApp",
                    "Mux Original"
                }
            );
            cmbVideoCodec.SelectedIndex = 0;

            for (int i = 0; i < languages.GetLength(0); i++)
                cmbLanguage.Items.Add(languages[i, 1]);

            cmbLanguage.SelectedIndex = 0;

            cmbSourceFPS.Items.AddRange
            (
                new string []
                {
                    "23.976 / 24",
                    "25",
                    "29.97 / 30",
                    "59.94 / 60"
                }
            );
            cmbSourceFPS.SelectedIndex = 0;

            cmbKeyframeInterval.Items.AddRange
            (
                new string []
                {
                    "2 Seconds",
                    "5 Seconds",
                    "10 Seconds"
                }
            );
            cmbKeyframeInterval.SelectedIndex = 0;

            cmbChannels.Items.AddRange
            (
                new string []
                {
                    "2 Channels",
                    "5.1 Channels",
                    "7.1 Channels"
                }
            );
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
            
            cbxVideo.Enabled = type != (int) ExtensionTypes.AUDIO;
            cbxVideo.Checked = type == (int) ExtensionTypes.VIDEO;
            cbxAudio.Enabled = type != (int) ExtensionTypes.VIDEO;
            cbxAudio.Checked = type == (int) ExtensionTypes.AUDIO;
            cbxMP4.Enabled   = type != (int) ExtensionTypes.AUDIO;
            cbxMKV.Enabled   = type != (int) ExtensionTypes.AUDIO;
        }

        string GetLanguageCode() { return languages[cmbLanguage.SelectedIndex, 0]; }

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
                input.common.ScriptFile = $"{input.common.OutputDir}Script.avs";
                EnableEncodeAndContainer();

                btnOpenFile.Enabled = false;
                btnNew.Enabled = true;
            }

            txbInFile.Text = input?.common.FileName;
            txbOutFile.Text = input is not null ? input.common.ScriptFile : home;
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            input.common.OutputDir = fbd.ShowDialog() == DialogResult.OK ? $@"{fbd.SelectedPath}\{input?.common.FileNameOnly}\" : input.common.OutputDir;
            input.common.ScriptFile = input is not null ? $"{input.common.OutputDir}Script.avs" : input.common.OutputDir;
            
            txbOutFile.Text = input.common.ScriptFile;
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            if (input is not null)
            {
                Directory.CreateDirectory(input.common.OutputDir);

                input.common.Video = cbxVideo.Checked;
                input.common.MuxOriginalVideo = cmbVideoCodec.SelectedIndex == (int)VideoCodecs.Original;
                input.common.VideoCodec = cmbVideoCodec.SelectedIndex;
                input.common.SourceFPS = sourceFPS[cmbSourceFPS.SelectedIndex];
                input.common.KeyframeIntervalInSeconds = kfInterval[cmbKeyframeInterval.SelectedIndex];
                input.common.NeedsToBeResized = cmbVideoCodec.SelectedIndex == (int)VideoCodecs.WhatsApp;
                
                input.common.Audio = cbxAudio.Checked;
                input.common.AudioCodec = cmbAudioCodec.SelectedIndex;
                input.common.AudioBitrate = (int)cmbBitrate.SelectedItem;
                input.common.AudioLanguage = GetLanguageCode();
                
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
            if (cmbVideoCodec.SelectedIndex == (int) VideoCodecs.Original && input?.common?.IsSupportedByMP4Box is false)
            {
                cbxMP4.Enabled = false;
                cbxMP4.Checked = false;
                cbxMKV.Checked = true;
            }
            else if (cmbVideoCodec.SelectedIndex == (int) VideoCodecs.Original && input?.common?.IsSupportedByMP4Box is true)
            {
                cbxMP4.Enabled = true;
            }
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
