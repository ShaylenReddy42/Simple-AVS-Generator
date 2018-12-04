/******************************************************************************
 * Copyright (C) 2018 Shaylen Reddy
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

namespace Simple_AVS_Generator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            txbOutFile.Text = outDir;
            PopulateComboBoxes();
        }

        static String home = "C:\\Users\\" + Environment.UserName + "\\Desktop\\Temp\\";

        String fileName = "",
                fileDir = "",
                      v = "",
                      a = "",
                 output = "",
                fileExt = "",
           fileNameOnly = "",
                 outDir = home;

        String cS = "*.3gp;*.3g2;*.mp4;*.mkv;*.avi;*.mov;*.m4v;*.flv",
               vS = "*.264;*.265;*.vp9",
               aS = "*.aac;*.m1a;*.m2a;*.mp3;*.m4a;*.dts;*.ac3;*.opus";

        #region Enums
        enum Video
        {
            HEVC = 0,
            AVC = 1,
            WhatsApp = 2,
            Original = 3
        }

        enum Audio
        {
            AAC_LC = 0,
            AAC_HE = 1,
            OPUS = 2
        }

        enum AudioLanguages
        {
            English = 0,
            Hindi = 1,
            Japanese = 2,
            Tamil = 3
        }

        enum AudioChannels
        {
            Two = 0,
            Six = 1,
            Eight = 2
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
            cmbVideoCodec.Items.Add("AVC");
            cmbVideoCodec.Items.Add("WhatsApp");
            cmbVideoCodec.Items.Add("Mux Original");
            cmbVideoCodec.SelectedIndex = 0;

            cmbLanguage.Items.Add("English");
            cmbLanguage.Items.Add("Hindi");
            cmbLanguage.Items.Add("Japanese");
            cmbLanguage.Items.Add("Tamil");
            cmbLanguage.SelectedIndex = 0;

            cmbChannels.Items.Add("2 Channels");
            cmbChannels.Items.Add("5.1 Channels");
            cmbChannels.Items.Add("7.1 Channels");
            cmbChannels.SelectedIndex = 0;
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
                ".VP9"
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
                ".AAC", ".M4A", //AAC Audio
                ".M1A", ".M2A", ".MP3", //MPEG Audio
                ".DTS",
                ".AC3", //Dolby Digital
                ".OPUS"
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

        void WriteFile(String outputFileName, String fileContents)
        {
            StreamWriter sw = new StreamWriter(outputFileName);
            sw.Write(fileContents);
            sw.Close();
        }

        bool SupportedInMP4()
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

                //Containers
                ".AVI",
                ".MPG", ".MPEG", ".VOB", ".VCD", ".SVCD", //MPEG-2 PS
                ".TS", ".M2T", //MPEG-2 TS
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

        int DetermineAudioBitrate()
        {
            int audioBitrate = 0;

            switch (cmbChannels.SelectedIndex)
            {
                case (int) AudioChannels.Two:
                    audioBitrate = cmbAudioCodec.SelectedIndex == (int) Audio.AAC_LC ? 112 :
                                   cmbAudioCodec.SelectedIndex == (int) Audio.AAC_HE ? 80 :
                                   96; //OPUS
                    break;
                case (int) AudioChannels.Six:
                    audioBitrate = cmbAudioCodec.SelectedIndex == (int) Audio.AAC_LC ? 320 :
                                   cmbAudioCodec.SelectedIndex == (int) Audio.AAC_HE ? 224 :
                                   288; //OPUS
                    break;
                case (int) AudioChannels.Eight:
                    audioBitrate = cmbAudioCodec.SelectedIndex == (int) Audio.AAC_LC ? 448 :
                                   cmbAudioCodec.SelectedIndex == (int) Audio.AAC_HE ? 320 :
                                   384; //OPUS
                    break;
            }

            return audioBitrate;
        }

        String DetermineAudioLanguage()
        {
            String audioLanguage = "";

            switch (cmbLanguage.SelectedIndex)
            {
                case (int) AudioLanguages.English:
                    audioLanguage = "eng";
                    break;
                case (int) AudioLanguages.Hindi:
                    audioLanguage = "hin";
                    break;
                case (int) AudioLanguages.Japanese:
                    audioLanguage = "jpn";
                    break;
                case (int) AudioLanguages.Tamil:
                    audioLanguage = "tam";
                    break;
            }

            return audioLanguage;
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
            cbxMP4.Enabled      = !audioExt;
            cbxMKV.Enabled      = !audioExt;
        }

        void New()
        {
            txbInFile.Clear();
            
            fileName = ""; fileNameOnly = ""; fileDir = ""; v = ""; a = ""; output = ""; fileExt = "";
            outDir = home;

            txbOutFile.Text = outDir;

            cbxMP4.Checked  = false; cbxMKV.Checked  = false;
            cbxVideo.Checked = false; cbxAudio.Checked = false;

            cbxVideo.Enabled = false; cbxAudio.Enabled = false;
            cbxMP4.Enabled  = false; cbxMKV.Enabled  = false;

            cmbVideoCodec.Enabled = false; cmbAudioCodec.Enabled = false;
            cmbVideoCodec.SelectedIndex = 0; cmbAudioCodec.SelectedIndex = 0;

            cmbLanguage.Enabled = false; cmbChannels.Enabled = false;
            cmbLanguage.SelectedIndex = 0; cmbChannels.SelectedIndex = 0;

            btnNew.Enabled  = false; btnOpenFile.Enabled = true;
        }

        void AVSMeter()
        {
            String switches = "-i -l";

            String outputFileName = outDir + "AVSMeter.cmd",
                     fileContents = "AVSMeter64 \"%~dp0Script.avs\" " + switches;

            WriteFile(outputFileName, fileContents);
        }

        void Encode(bool video, bool audio)
        {
            if (video)
            {
                String vPipe = "avs2pipemod -y4mp \"%~dp0Script.avs\" | ",
                    vEncoder = "",
                    vCmdFile = outDir;

                if (cmbVideoCodec.SelectedIndex == (int) Video.HEVC)
                {
                    vEncoder += "x265 -P main --preset slower --crf 27 -i 1 -I 48 --scenecut-bias 10 --bframes 1 ";
                    vEncoder += "--aq-mode 3 --aq-motion --aud --no-open-gop --y4m -f 0 - \"%~dp0Video.265\"";
                    vCmdFile += "Encode Video [HEVC].cmd";
                }
                else if (cmbVideoCodec.SelectedIndex == (int) Video.AVC)
                {
                    vEncoder += "x264 --preset slower --crf 27 -i 1 -I 48 --bframes 1 --aq-mode 3 --aud --no-mbtree ";
                    vEncoder += "--demuxer y4m --frames 0 -o \"%~dp0Video.264\" -";
                    vCmdFile += "Encode Video [AVC].cmd";
                }
                else if (cmbVideoCodec.SelectedIndex == (int) Video.WhatsApp)
                {
                    vEncoder += "x264 --preset slower --crf 27 -i 1 -I 10000 --bframes 16 --no-scenecut --aud --no-mbtree ";
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
                    aEncoder += "qaac64 --abr " + DetermineAudioBitrate() + " --ignorelength --no-delay ";
                    aEncoder += "-o \"%~dp0" + fileNameOnly + ".m4a\" - ";
                    aCmdFile += "Encode Audio [AAC-LC].cmd";
                }
                else if (cmbAudioCodec.SelectedIndex == (int) Audio.AAC_HE)
                {
                    aEncoder += "qaac64 --he --abr " + DetermineAudioBitrate() + " --ignorelength ";
                    aEncoder += "-o \"%~dp0" + fileNameOnly + ".m4a\" - ";
                    aCmdFile += "Encode Audio [AAC-HE].cmd";
                }
                else //OPUS
                {
                    aEncoder += "opusenc --bitrate " + DetermineAudioBitrate() + " --ignorelength ";
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

            if (cmbVideoCodec.SelectedIndex == (int) Video.WhatsApp)
            {
                fileContents += "# Calculate the target height based on a target width" + "\r\n";
                fileContents += "aspectRatio  = float(Width(v)) / float(Height(v))" + "\r\n";
                fileContents += "targetWidth  = 480" + "\r\n";
                fileContents += "targetHeight = int(targetWidth / aspectRatio)" + "\r\n";
                fileContents += "targetHeight = targetHeight + ((targetHeight % 2 != 0) ? 1 : 0)";
                fileContents += "\r\n\r\n";

                fileContents += "v = Spline36Resize(v, targetWidth, targetHeight)";
                fileContents += "\r\n\r\n";
            }

            return fileContents;
        }

        void OutputContainer(bool mp4, bool mkv, bool originalVideo)
        {
            String videoExtension = cmbVideoCodec.SelectedIndex == (int) Video.HEVC ? ".265" : ".264",
                   audioExtension = cmbAudioCodec.SelectedIndex == (int) Audio.OPUS ? ".ogg" : ".m4a",

                   outputFileName = outDir,
                     fileContents = "";

            if (mp4)
            {
                String mp4V = !originalVideo ? "-add \"%~dp0Video" + videoExtension + "\":name= " :
                                               "-add \"" + fileName + "\"#video ",
                       mp4A = cbxAudio.Checked ? "-add \"%~dp0" + fileNameOnly + ".m4a\":name=:lang=" + DetermineAudioLanguage() : "",
                     newmp4 = " -new " + "\"%~dp0" + fileNameOnly + ".mp4\"";

                outputFileName += "MP4 Mux" + (originalVideo ? " [Original Video]" : "") + ".cmd";
                fileContents = "mp4box " + mp4V + mp4A + newmp4;
            }
            else if (mkv)
            {
                String mkvO = "-o \"%~dp0" + fileNameOnly + ".mkv\" ",
                       mkvV = !originalVideo ? "\"%~dp0Video" + videoExtension + "\" " :
                                               "--no-audio \"" + fileName + "\" ",
                       mkvA = cbxAudio.Checked ? "--language 0:eng \"%~dp0" + fileNameOnly + audioExtension + "\" " : "";

                outputFileName += "MKV Mux" + (originalVideo ? " [Original Video]" : "") + ".cmd";
                fileContents = "mkvmerge " + mkvO + mkvV + mkvA;
            }

            WriteFile(outputFileName, fileContents);
        }
        #endregion Methods

        #region Buttons
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            String sF = "All Supported|" + cS + ";" + vS + ";" + aS,
                   cF = "Container Types [3GP 3G2 AVI FLV MP4 MKV MOV M4V]|" + cS,
                   vF = "Video Types [264 265 VP9]|" + vS,
                   aF = "Audio Types [AAC AC3 DTS M1A M2A MP3 M4A]|" + aS;

            OpenFileDialog ofd = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open File",
                Filter = sF + "|" + cF + "|" + vF + "|" + aF
            };

            fileName = ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : "";

            if (fileName != "")
            {
                fileNameOnly = ofd.SafeFileName;
                fileNameOnly = fileNameOnly.Substring(0, fileNameOnly.Length - 4);
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
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileName != "" && Directory.Exists(outDir) && !File.Exists(output))
                Directory.Delete(outDir);
        }

        private void cbxVideo_CheckedChanged(object sender, EventArgs e)
        {
            cbxMP4.Checked = cbxVideo.Checked &&
                             cbxMP4.Enabled &&
                             cmbAudioCodec.SelectedIndex != (int) Audio.OPUS &&
                             !cbxMKV.Checked;

            cmbVideoCodec.Enabled = cbxVideo.Checked;
        }

        private void cbxAudio_CheckedChanged(object sender, EventArgs e)
        {
            cmbAudioCodec.Enabled = cbxAudio.Checked;
        }
        
        private void cmbVideoCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVideoCodec.SelectedIndex == (int) Video.Original && !SupportedInMP4())
            {
                cbxMP4.Enabled = false;
                cbxMP4.Checked = false;
                cbxMKV.Checked = true;
            }
            else if (cmbAudioCodec.SelectedIndex != (int) Audio.OPUS &&
                     fileName != "")
                cbxMP4.Enabled = true;
        }

        private void cmbAudioCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxVideo.Enabled)
            {
                if (cmbAudioCodec.SelectedIndex == (int) Audio.OPUS)
                {
                    cbxMP4.Enabled = false;
                    cbxMP4.Checked = false;
                    cbxMKV.Checked = true;
                }
                else if (cmbAudioCodec.SelectedIndex != (int) Audio.OPUS &&
                         (cmbVideoCodec.SelectedIndex != (int) Video.Original ||
                         (cmbVideoCodec.SelectedIndex == (int) Video.Original && SupportedInMP4())))
                    cbxMP4.Enabled = true;
            }
        }

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
        #endregion ComponentEvents
    }
}
