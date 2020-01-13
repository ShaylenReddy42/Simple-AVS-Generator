/******************************************************************************
 * Copyright (C) 2018-2020 Shaylen Reddy
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

using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;

namespace Simple_AVS_Generator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            lbltitle.Text += " v" + FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion;

            if (global::Properties.Settings.Default.Location.X <= 0 ||
                global::Properties.Settings.Default.Location.Y <= 0)
                this.StartPosition = FormStartPosition.CenterScreen;
            else
                this.DataBindings.Add("Location", global::Properties.Settings.Default, "Location", true, DataSourceUpdateMode.OnPropertyChanged);

            txbOutFile.Text = outDir;
            PopulateComboBoxes();
        }

        //Variables for dragging the form
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        static String home = "C:\\Users\\" + Environment.UserName + "\\Desktop\\Temp\\";

        String fileName = "",
                fileDir = "",
                      v = "",
                      a = "",
                 output = "",
                fileExt = "",
           fileNameOnly = "",
                 outDir = home;

        String supportedContainerExts = "*.3gp;*.3g2;*.asf;*.avi;*.flv;*.mp4;*.m4v;*.mkv;*.mov;*.m2t;*.m2ts;*.mxf;*.ogm;*.rm;*.rmvb;*.ts;*.wmv",
                   supportedVideoExts = "*.263;*.h263;*.264;*.h264;*.265;*.h265;*.hevc;*.y4m",
                   supportedAudioExts = "*.aa3;*.aac;*.aif;*.ac3;*.ape;*.dts;*.flac;*.m1a;*.m2a;*.mp2;*.mp3;*.m4a;*.oma;*.opus;*.thd;*.tta;*.wav;*.wma";

        #region Languages
        String [,] languages =
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
            { 112, 320, 448 }, //AAC-LC
            {  80, 192, 256 }, //AAC-HE
            {  96, 288, 384 }  //OPUS
        };
        #endregion AudioBitrates

        #region Enums
        enum Video
        {
            HEVC = 0,
            AV1 = 1,
            AVC = 2,
            WhatsApp = 3,
            Original = 4
        }

        enum Audio
        {
            AAC_LC = 0,
            AAC_HE = 1,
            OPUS = 2
        }
        #endregion Enums

        #region Methods
        void PopulateComboBoxes()
        {
            cmbAudioCodec.Items.Add("AAC-LC");
            cmbAudioCodec.Items.Add("AAC-HE");
            cmbAudioCodec.Items.Add("OPUS");
            cmbAudioCodec.SelectedIndex = 0;

            cmbVideoCodec.Items.Add("HEVC");
            cmbVideoCodec.Items.Add("AV1");
            cmbVideoCodec.Items.Add("AVC");
            cmbVideoCodec.Items.Add("WhatsApp");
            cmbVideoCodec.Items.Add("Mux Original");
            cmbVideoCodec.SelectedIndex = 0;

            for (int i = 0; i < languages.GetLength(0); i++)
                cmbLanguage.Items.Add(languages[i, 1]);

            cmbLanguage.SelectedIndex = 0;

            cmbChannels.Items.Add("2 Channels");
            cmbChannels.Items.Add("5.1 Channels");
            cmbChannels.Items.Add("7.1 Channels");
            cmbChannels.SelectedIndex = 0;
            
            SetSelectableAudioBitrates();
        }

        void SetSelectableAudioBitrates()
        {
            cmbBitrate.Items.Clear();

            int audioCodec = cmbAudioCodec.SelectedIndex,
             audioChannels = cmbChannels.SelectedIndex == -1 ? 0 : cmbChannels.SelectedIndex;

            for (int i = 0; i < selectableAudioBitrates.GetLength(2); i++)
                cmbBitrate.Items.Add(selectableAudioBitrates[audioCodec, audioChannels, i]);

            cmbBitrate.SelectedItem = defaultAudioBitrates[audioCodec, audioChannels];
        }

        void EnableEncodeAndContainer()
        {
            bool videoExt = VideoExt(),
                 audioExt = AudioExt();

            cbxVideo.Enabled    = !audioExt;
            cbxVideo.Checked    =  videoExt;
            cbxAudio.Enabled    = !videoExt;
            cbxAudio.Checked    =  audioExt;
            cmbChannels.Enabled = !videoExt;
            cmbLanguage.Enabled = !videoExt && !audioExt;
            cmbBitrate.Enabled  = !videoExt;
            cbxMP4.Enabled      = !audioExt;
            cbxMKV.Enabled      = !audioExt;
        }

        bool VideoExt()
        {
            bool isVideoExt = false;

            String [] videoExts =
            {
                ".M1V", ".M2V", //MPEG-1-2 Video
                ".CMP", ".M4V", //MPEG-4 Video
                ".263", ".H263", //H263 Video
                ".H264", ".H26L", ".264", ".26L", ".X264", ".SVC", //AVC Video
                ".HEVC", ".H265", ".265", ".HVC", ".SHVC", ".LHVC", ".MHVC", //HEVC Video
                ".Y4M" //YUV4MPEG Raw Video
            };

            foreach (String ext in videoExts)
            {
                if (fileExt.Equals(ext))
                {
                    isVideoExt = true;
                    break;
                }
            }

            return isVideoExt;
        }

        bool AudioExt()
        {
            bool isAudioExt = false;

            String [] audioExts =
            {
                ".AA3", ".OMA", //Advanced Transform Acoustic Coding [ATRAC] Audio
                ".AAC", ".M4A", //Advanced Audio Coding [AAC] Audio
                ".AC3", ".THD", //Dolby Digital
                ".AIF", //AIFF Audio
                ".APE", //Monkey Audio
                ".DTS",
                ".FLAC", //Free Lossless Audio Codec
                ".M1A", ".M2A", ".MP2", ".MP3", //MPEG Audio
                ".OPUS",
                ".TTA",
                ".WAV",
                ".WMA"
            };

            foreach (String ext in audioExts)
            {
                if (fileExt.Equals(ext))
                {
                    isAudioExt = true;
                    break;
                }
            }

            return isAudioExt;
        }

        bool SupportedByMP4Box()
        {
            bool supported = false;

            String [] supportedExts =
            {
                //Raw video extensions
                ".M1V", ".M2V", //MPEG-1-2 Video
                ".CMP", ".M4V", //MPEG-4 Video
                ".263", ".H263", //H263 Video
                ".H264", ".H26L", ".264", ".26L", ".X264", ".SVC", //AVC Video
                ".HEVC", ".H265", ".265", ".HVC", ".SHVC", ".LHVC", ".MHVC", //HEVC Video
                ".IVF", //AV1 and VP9 Video
                ".OBU", //AV1 Video

                //Containers
                ".AVI",
                ".MPG", ".MPEG", ".VOB", ".VCD", ".SVCD", //MPEG-2 Program Streams
                ".TS", ".M2T", ".M2TS", //MPEG-2 Transport Streams
                ".QCP",
                ".OGG",
                ".MP4", ".3GP", ".3G2" //Some ISO Media Extensions
            };

            foreach (String ext in supportedExts)
            {
                if (fileExt.Equals(ext))
                {
                    supported = true;
                    break;
                }
            }

            return supported;
        }
        
        String GetLanguageCode() { return languages[cmbLanguage.SelectedIndex, 0]; }

        int GetAudioBitrate() { return (int) cmbBitrate.SelectedItem; }

        void Encode(bool video, bool audio)
        {
            if (video)
            {
                String vPipe = "avs2pipemod -y4mp \"%~dp0Script.avs\" | ",
                    vEncoder = "",
                    vCmdFile = outDir;

                if (cmbVideoCodec.SelectedIndex == (int) Video.HEVC)
                {
                    vEncoder += "x265 -P main --preset slower --crf 26 -i 1 -I 48 --scenecut-bias 10 --bframes 1 ";
                    vEncoder += "--aq-mode 3 --aq-motion --aud --no-open-gop --y4m -f 0 - \"%~dp0Video.265\"";
                    vCmdFile += "Encode Video [HEVC].cmd";
                }
                else if (cmbVideoCodec.SelectedIndex == (int) Video.AV1)
                {
                    vEncoder += "aomenc --passes=1 --end-usage=q --min-q=26 --max-q=34 --target-bitrate=0 --lag-in-frames=10 ";
                    vEncoder += "--enable-fwd-kf=1 --kf-max-dist=48 --verbose --ivf -o \"%~dp0Video.ivf\" -";
                    vCmdFile += "Encode Video [AV1].cmd";
                }
                else if (cmbVideoCodec.SelectedIndex == (int) Video.AVC)
                {
                    vEncoder += "x264 --preset slower --crf 26 -i 1 -I 48 --bframes 1 --aq-mode 3 --aud --no-mbtree ";
                    vEncoder += "--demuxer y4m --frames 0 -o \"%~dp0Video.264\" -";
                    vCmdFile += "Encode Video [AVC].cmd";
                }
                else if (cmbVideoCodec.SelectedIndex == (int) Video.WhatsApp)
                {
                    vEncoder += "x264 --preset slower --crf 26 -i 1 -I 10000 --bframes 8 --no-scenecut --aud --no-mbtree ";
                    vEncoder += "--demuxer y4m --frames 0 -o \"%~dp0Video.264\" -";
                    vCmdFile += "Encode Video [AVC].cmd";
                }

                String outputFileName = vCmdFile,
                         fileContents = vPipe + vEncoder;

                WriteFile(outputFileName, fileContents);
                AVSMeter();
            }

            if (audio)
            {
                String aPipe = "avs2pipemod -wav=16bit \"%~dp0Script.avs\" | ",
                    aEncoder = "",
                    aCmdFile = outDir;

                if (cmbAudioCodec.SelectedIndex == (int) Audio.AAC_LC)
                {
                    aEncoder += "qaac64 --abr " + GetAudioBitrate() + " --ignorelength --no-delay ";
                    aEncoder += "-o \"%~dp0" + fileNameOnly + ".m4a\" - ";
                    aCmdFile += "Encode Audio [AAC-LC].cmd";
                }
                else if (cmbAudioCodec.SelectedIndex == (int) Audio.AAC_HE)
                {
                    aEncoder += "qaac64 --he --abr " + GetAudioBitrate() + " --ignorelength ";
                    aEncoder += "-o \"%~dp0" + fileNameOnly + ".m4a\" - ";
                    aCmdFile += "Encode Audio [AAC-HE].cmd";
                }
                else //OPUS
                {
                    aEncoder += "opusenc --bitrate " + GetAudioBitrate() + " --ignorelength ";
                    aEncoder += "- \"%~dp0" + fileNameOnly + ".ogg\"";
                    aCmdFile += "Encode Audio [OPUS].cmd";
                }

                String outputFileName = aCmdFile,
                         fileContents = aPipe + aEncoder;

                WriteFile(outputFileName, fileContents);
            }
        }

        String ResizeVideo()
        {
            String fileContents = "";

            fileContents += "# Calculate the target height based on a target width" + "\r\n";
            fileContents += "aspectRatio  = float(Width(v)) / float(Height(v))" + "\r\n";
            fileContents += "targetWidth  = " + (cmbVideoCodec.SelectedIndex == (int) Video.WhatsApp ? "480" : "Width(v)") + "\r\n";
            fileContents += "targetHeight = int(targetWidth / aspectRatio)" + "\r\n";
            fileContents += "targetHeight = targetHeight + ((targetHeight % 2 != 0) ? 1 : 0)";
            fileContents += "\r\n\r\n";

            fileContents += "v = Spline36Resize(v, targetWidth, targetHeight)";
            fileContents += "\r\n\r\n";

            return fileContents;
        }

        void OutputContainer(bool mp4, bool mkv, bool originalVideo)
        {
            String videoExtension = cmbVideoCodec.SelectedIndex == (int) Video.HEVC ? ".265" :
                                    cmbVideoCodec.SelectedIndex == (int) Video.AV1 ? ".ivf" :
                                    ".264",
                   audioExtension = cmbAudioCodec.SelectedIndex == (int) Audio.OPUS ? ".ogg" : ".m4a",

                   outputFileName = outDir,
                     fileContents = "";

            if (mp4)
            {
                String mp4V = !originalVideo ? "-add \"%~dp0Video" + videoExtension + "\":name= " :
                                               "-add \"" + fileName + "\"#video ",
                       mp4A = cbxAudio.Checked ? "-add \"%~dp0" + fileNameOnly + audioExtension + "\":name=:lang=" + GetLanguageCode() : "",
                     newmp4 = " -new " + "\"%~dp0" + fileNameOnly + ".mp4\"";

                outputFileName += "MP4 Mux" + (originalVideo ? " [Original Video]" : "") + ".cmd";
                fileContents = "mp4box " + mp4V + mp4A + newmp4;
            }
            else if (mkv)
            {
                String mkvO = "-o \"%~dp0" + fileNameOnly + ".mkv\" ",
                       mkvV = !originalVideo ? "\"%~dp0Video" + videoExtension + "\" " :
                                               "--no-audio \"" + fileName + "\" ",
                       mkvA = cbxAudio.Checked ? "--language 0:" + GetLanguageCode() + " \"%~dp0" + fileNameOnly + audioExtension + "\" " : "";

                outputFileName += "MKV Mux" + (originalVideo ? " [Original Video]" : "") + ".cmd";
                fileContents = "mkvmerge " + mkvO + mkvV + mkvA;
            }

            WriteFile(outputFileName, fileContents);
        }

        void AVSMeter()
        {
            String switches = "-i -l";

            String outputFileName = outDir + "AVSMeter.cmd",
                     fileContents = "AVSMeter64 \"%~dp0Script.avs\" " + switches;

            WriteFile(outputFileName, fileContents);
        }

        void WriteFile(String outputFileName, String fileContents)
        {
            StreamWriter sw = new StreamWriter(outputFileName);
            sw.Write((outputFileName.EndsWith(".cmd") ? "@ECHO off\r\n\r\n" : "") + fileContents);
            sw.Close();
        }

        void New()
        {
            txbInFile.Clear();

            fileName = "";
            fileNameOnly = "";
            fileDir = "";
            v = "";
            a = "";
            output = "";
            fileExt = "";
            outDir = home;

            txbOutFile.Text = outDir;

            cbxVideo.Checked = false;
            cbxVideo.Enabled = false;

            cmbVideoCodec.Enabled = false;
            cmbVideoCodec.SelectedIndex = 0;

            cbxAudio.Enabled = false;
            cbxAudio.Checked = false;

            cmbAudioCodec.Enabled = false;
            cmbAudioCodec.SelectedIndex = 0;

            cmbLanguage.Enabled = false;
            cmbLanguage.SelectedIndex = 0;

            cmbChannels.Enabled = false;
            cmbChannels.SelectedIndex = 0;

            cmbBitrate.Enabled = false;

            cbxMP4.Enabled = false;
            cbxMP4.Checked = false;

            cbxMKV.Enabled = false;
            cbxMKV.Checked = false;

            btnNew.Enabled = false;
            btnOpenFile.Enabled = true;
        }
        #endregion Methods

        #region Buttons
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            String filterSupportedExts = "All Supported|" + supportedContainerExts + ";" + supportedVideoExts + ";" + supportedAudioExts,
                   filterContainerExts = "Container Types [3GP 32G ASF AVI FLV M4V MP4 MKV MOV M2T M2TS MXF OGM RM RMVB TS WMV]|" + supportedContainerExts,
                       filterVideoExts = "Video Types [263 H263 264 H264 265 H265 HEVC Y4M]|" + supportedVideoExts,
                       filterAudioExts = "Audio Types [AA3 AAC AC3 AIF APE DTS FLAC M1A M2A MP2 MP3 M4A OMA OPUS THD TTA WAV WMA]|" + supportedAudioExts;

            OpenFileDialog ofd = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open File",
                Filter = filterSupportedExts + "|" + filterContainerExts + "|" + filterVideoExts + "|" + filterAudioExts
            };

            fileName = ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : "";

            if (fileName != "")
            {
                fileNameOnly = ofd.SafeFileName;
                fileNameOnly = fileNameOnly.Substring(0, fileNameOnly.LastIndexOf('.'));
                fileDir = fileName.Substring(0, fileName.LastIndexOf("\\"));
                fileExt = fileName.Substring(fileName.LastIndexOf('.')).ToUpper();
                outDir = outDir + fileNameOnly + "\\";
                output = outDir + "Script.avs";
                EnableEncodeAndContainer();

                btnOpenFile.Enabled = false; btnNew.Enabled = true;
            }

            txbInFile.Text = fileName;
            txbOutFile.Text = fileName != "" ? output : outDir;
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            outDir = fbd.ShowDialog() == DialogResult.OK ? fbd.SelectedPath + "\\" + fileNameOnly + "\\" : outDir;
            output = fileName != "" ? outDir + "Script.avs" : outDir;
            
            txbOutFile.Text = output;
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(outDir);

            if (fileName != "")
            {
                String i = "i = \"" + fileName + "\"";
                v = cbxVideo.Checked & cmbVideoCodec.SelectedIndex != (int) Video.Original ? "v = LWLibavVideoSource(i).ConvertToYV12()" : "";

                a = cbxAudio.Checked ? "a = LWLibavAudioSource(i).ConvertAudioToFloat()" : "";

                String outputFileName = output,
                         fileContents = i + "\r\n\r\n";

                if (v == "" && a != "") //Audio only
                {
                    fileContents += a;
                    fileContents += "\r\n\r\n";

                    fileContents += "a = Normalize(a, 1.0)";
                    fileContents += "\r\n\r\n";

                    fileContents += "a = ConvertAudioTo16Bit(a)";
                    fileContents += "\r\n\r\n";

                    fileContents += "a";

                    Encode(false, true);
                }
                else if (v != "" && a == "") //Video only
                {
                    fileContents += v;
                    fileContents += "\r\n\r\n";

                    fileContents += ResizeVideo();

                    fileContents += "v";

                    Encode(true, false);
                }
                else if (v != "" && a != "") //Video and Audio
                {
                    fileContents += v;
                    fileContents += "\r\n\r\n";

                    fileContents += ResizeVideo();

                    fileContents += a;
                    fileContents += "\r\n\r\n";

                    fileContents += "a = Normalize(a, 1.0)";
                    fileContents += "\r\n\r\n";

                    fileContents += "o = AudioDub(v, a)";
                    fileContents += "\r\n\r\n";

                    fileContents += "o = ConvertAudioTo16Bit(o)";
                    fileContents += "\r\n\r\n";

                    fileContents += "o";

                    Encode(true, true);
                }

                WriteFile(outputFileName, fileContents);

                if (File.Exists(output))
                {
                    if (cbxVideo.Checked && (cbxMP4.Checked || cbxMKV.Checked))
                        OutputContainer(cbxMP4.Checked, cbxMKV.Checked, cmbVideoCodec.SelectedIndex == (int) Video.Original);

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
            if (fileName != "" && Directory.Exists(outDir) && !File.Exists(output))
                Directory.Delete(outDir);

            global::Properties.Settings.Default.Location = this.Location;
            global::Properties.Settings.Default.Save();
        }

        private void cbxVideo_CheckedChanged(object sender, EventArgs e)
        {
            cbxMP4.Checked = cbxVideo.Checked &&
                             cbxMP4.Enabled &&
                             !cbxMKV.Checked;

            cmbVideoCodec.Enabled = cbxVideo.Checked;
        }

        private void cbxAudio_CheckedChanged(object sender, EventArgs e)
        {
            cmbAudioCodec.Enabled = cbxAudio.Checked;
        }
        
        private void cmbVideoCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVideoCodec.SelectedIndex == (int) Video.Original && !SupportedByMP4Box())
            {
                cbxMP4.Enabled = false;
                cbxMP4.Checked = false;
                cbxMKV.Checked = true;
            }
            else if (fileName != "")
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
