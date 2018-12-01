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
            populateComboLists();
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
        
        #region Methods
        void populateComboLists()
        {
            cbxAudioCodec.Items.Add("AAC-LC");
            cbxAudioCodec.Items.Add("AAC-HE");
            cbxAudioCodec.Items.Add("OPUS");
            cbxAudioCodec.SelectedIndex = 0;

            cbxVideoCodec.Items.Add("HEVC");
            cbxVideoCodec.Items.Add("AVC");
            cbxVideoCodec.Items.Add("Whatsapp");
            cbxVideoCodec.Items.Add("Mux Original");
            cbxVideoCodec.SelectedIndex = 0;

            cbxLanguage.Items.Add("English");
            cbxLanguage.Items.Add("Hindi");
            cbxLanguage.Items.Add("Japanese");
            cbxLanguage.Items.Add("Tamil");
            cbxLanguage.SelectedIndex = 0;

            cbxChannels.Items.Add("2 Channels");
            cbxChannels.Items.Add("5.1 Channels");
            cbxChannels.Items.Add("7.1 Channels");
            cbxChannels.SelectedIndex = 0;
        }

        void writeFile(String outputFileName, String fileContents)
        {
            StreamWriter sw = new StreamWriter(outputFileName);
            sw.Write(fileContents);
            sw.Close();
        }

        int determineAudioBitrate()
        {
            int audioBitrate = 0;

            switch (cbxChannels.SelectedIndex)
            {
                case (int) AudioChannels.Two:
                    audioBitrate = cbxAudioCodec.SelectedIndex == (int) Audio.AAC_LC ? 112 :
                                   cbxAudioCodec.SelectedIndex == (int) Audio.AAC_HE ? 80 :
                                   96; //OPUS
                    break;
                case (int) AudioChannels.Six:
                    audioBitrate = cbxAudioCodec.SelectedIndex == (int) Audio.AAC_LC ? 320 :
                                   cbxAudioCodec.SelectedIndex == (int) Audio.AAC_HE ? 224 :
                                   288; //OPUS
                    break;
                case (int) AudioChannels.Eight:
                    audioBitrate = cbxAudioCodec.SelectedIndex == (int) Audio.AAC_LC ? 448 :
                                   cbxAudioCodec.SelectedIndex == (int) Audio.AAC_HE ? 320 :
                                   384; //OPUS
                    break;
            }

            return audioBitrate;
        }

        String determineAudioLanguage()
        {
            String audioLanguage = "";

            switch (cbxLanguage.SelectedIndex)
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
            chkVEnc.Enabled     = true;
            chkAEnc.Enabled     = true;
            cbxChannels.Enabled = true;
            cbxLanguage.Enabled = true;
            chbMP4.Enabled      = true;
            chbMKV.Enabled      = true;
        }

        void New()
        {
            txbInFile.Clear();
            
            fileName = ""; fileNameOnly = ""; fileDir = ""; v = ""; a = ""; output = ""; fileExt = "";
            outDir = home;

            txbOutFile.Text = outDir;

            chbMP4.Checked  = false; chbMKV.Checked  = false;
            chkVEnc.Checked = false; chkAEnc.Checked = false;

            chkVEnc.Enabled = false; chkAEnc.Enabled = false;
            chbMP4.Enabled  = false; chbMKV.Enabled  = false;

            cbxVideoCodec.Enabled = false; cbxAudioCodec.Enabled = false;
            cbxVideoCodec.SelectedIndex = 0; cbxAudioCodec.SelectedIndex = 0;

            cbxLanguage.Enabled = false; cbxChannels.Enabled = false;
            cbxLanguage.SelectedIndex = 0; cbxChannels.SelectedIndex = 0;

            btnNew.Enabled  = false; btnOpenFile.Enabled = true;
        }

        void avsMeter()
        {
            String switches = "-i -l";

            String outputFileName = outDir + "AVSMeter.cmd",
                     fileContents = "AVSMeter64 \"%~dp0Script.avs\" " + switches;

            writeFile(outputFileName, fileContents);
        }

        void Encode(bool video, bool audio)
        {
            if (video)
            {
                String vPipe = "avs2pipemod -y4mp \"%~dp0Script.avs\" | ",
                    vEncoder = "",
                    vCmdFile = outDir;

                if (cbxVideoCodec.SelectedIndex == (int) Video.HEVC)
                {
                    vEncoder += "x265 -P main --preset slower --crf 27 -i 1 -I 48 --scenecut-bias 10 --bframes 1 ";
                    vEncoder += "--aq-mode 3 --aq-motion --aud --no-open-gop --y4m -f 0 - \"%~dp0Video.265\"";
                    vCmdFile += "Encode Video [HEVC].cmd";
                }
                else if (cbxVideoCodec.SelectedIndex == (int) Video.AVC)
                {
                    vEncoder += "x264 --preset slower --crf 27 -i 1 -I 48 --bframes 1 --aq-mode 3 --aud --no-mbtree ";
                    vEncoder += "--demuxer y4m --frames 0 -o \"%~dp0Video.264\" -";
                    vCmdFile += "Encode Video [AVC].cmd";
                }
                else if (cbxVideoCodec.SelectedIndex == (int) Video.WhatsApp)
                {
                    vEncoder += "x264 --preset slower --crf 27 -i 1 -I 10000 --bframes 16 --no-scenecut --aud --no-mbtree ";
                    vEncoder += "--demuxer y4m --frames 0 -o \"%~dp0Video.264\" -";
                    vCmdFile += "Encode Video [AVC].cmd";
                }

                String outputFileName = vCmdFile,
                         fileContents = vPipe + vEncoder;

                writeFile(outputFileName, fileContents);
                avsMeter();
            }

            if (audio)
            {
                String aPipe = "avs2pipemod -wav=16bit \"%~dp0Script.avs\" | ",
                    aEncoder = "",
                    aCmdFile = outDir;

                if (cbxAudioCodec.SelectedIndex == (int) Audio.AAC_LC)
                {
                    aEncoder += "qaac64 --abr " + determineAudioBitrate() + " --ignorelength --no-delay ";
                    aEncoder += "-o \"%~dp0" + fileNameOnly + ".m4a\" - ";
                    aCmdFile += "Encode Audio [AAC-LC].cmd";
                }
                else if (cbxAudioCodec.SelectedIndex == (int) Audio.AAC_HE)
                {
                    aEncoder += "qaac64 --he --abr " + determineAudioBitrate() + " --ignorelength ";
                    aEncoder += "-o \"%~dp0" + fileNameOnly + ".m4a\" - ";
                    aCmdFile += "Encode Audio [AAC-HE].cmd";
                }
                else //OPUS
                {
                    aEncoder += "opusenc --bitrate " + determineAudioBitrate() + " --ignorelength ";
                    aEncoder += "- \"%~dp0" + fileNameOnly + ".ogg\"";
                    aCmdFile += "Encode Audio [OPUS].cmd";
                }

                String outputFileName = aCmdFile,
                         fileContents = aPipe + aEncoder;

                writeFile(outputFileName, fileContents);
            }
        }

        void container(bool mp4, bool mkv)
        {
            String videoExtension = cbxVideoCodec.SelectedIndex == (int) Video.HEVC ? ".265" : ".264",
                   audioExtension = cbxAudioCodec.SelectedIndex == (int) Audio.OPUS ? ".ogg" : ".m4a",

                   outputFileName = "",
                     fileContents = "";

            if (mp4)
            {
                String mp4V = "-add \"%~dp0Video" + videoExtension + "\":name= ",
                       mp4A = chkAEnc.Checked ? "-add \"%~dp0" + fileNameOnly + ".m4a\":name=:lang=" + determineAudioLanguage() : "",
                     newmp4 = " -new " + "\"%~dp0" + fileNameOnly + ".mp4\"";

                outputFileName = outDir + "MP4 Mux.cmd";
                fileContents = "mp4box " + mp4V + mp4A + newmp4;
            }

            if (mkv)
            {
                String mkvO = "-o \"%~dp0" + fileNameOnly + ".mkv\" ",
                       mkvV = "\"%~dp0Video" + videoExtension + "\" ",
                       mkvA = chkAEnc.Checked ? "--language 0:eng \"%~dp0" + fileNameOnly + audioExtension + "\" " : "";

                outputFileName = outDir + "MKV Mux.cmd";
                fileContents = "mkvmerge " + mkvO + mkvV + mkvA;
            }

            if (cbxVideoCodec.SelectedIndex == (int) Video.Original)
            {
                String mp4V = "-add \"" + fileName + "\"#video ",
                       mp4A = chkAEnc.Checked ? "-add \"%~dp0" + fileNameOnly + ".m4a\":name=:lang=" + determineAudioLanguage() : "",
                     newmp4 = " -new " + "\"%~dp0" + fileNameOnly + ".mp4\"";

                outputFileName = outDir + "MP4 Mux [Original Video].cmd";
                fileContents = "mp4box " + mp4V + mp4A + newmp4;
            }

            writeFile(outputFileName, fileContents);
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
                EnableEncodeAndContainer();
                fileNameOnly = ofd.SafeFileName;
                fileNameOnly = fileNameOnly.Substring(0, fileNameOnly.Length - 4);
                fileDir = fileName.Substring(0, fileName.LastIndexOf("\\"));
                fileExt = fileName.Substring(fileName.LastIndexOf('.')).ToUpper();
                outDir = outDir + fileNameOnly + "\\";
                output = outDir + "Script.avs";

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
                String i = "i=\"" + fileName + "\"";
                v = chkVEnc.Checked & cbxVideoCodec.SelectedIndex != (int) Video.Original ? "v=LWLibavVideoSource(i).ConvertToYV12()" : "";
                v = cbxVideoCodec.SelectedIndex == (int) Video.WhatsApp ? v + ".Spline36Resize(480, 270)" : v;

                a = chkAEnc.Checked ? "a=LWLibavAudioSource(i).ConvertAudioToFloat()" : "";

                String outputFileName = output,
                         fileContents = "";

                if (v == "" && a != "")
                {
                    fileContents += i;
                    fileContents += "\r\n\r\n";

                    fileContents += a;
                    fileContents += "\r\n\r\n";

                    fileContents += "a=Normalize(a, 1.0)";
                    fileContents += "\r\n\r\n";

                    fileContents += "a=ConvertAudioTo16Bit(a)";
                    fileContents += "\r\n\r\n";

                    fileContents += "o";

                    Encode(false, true);
                }
                else if (a == "" && v != "")
                {
                    fileContents += i;
                    fileContents += "\r\n\r\n";

                    fileContents += v;
                    fileContents += "\r\n\r\n";

                    fileContents += "v";

                    Encode(true, false);
                }
                else if (v != "" && a != "")
                {
                    fileContents += i;
                    fileContents += "\r\n\r\n";

                    fileContents += v;
                    fileContents += "\r\n\r\n";

                    fileContents += a;
                    fileContents += "\r\n\r\n";

                    fileContents += "a=Normalize(a, 1.0)";
                    fileContents += "\r\n\r\n";

                    fileContents += "o=AudioDub(v, a)";
                    fileContents += "\r\n\r\n";

                    fileContents += "o=ConvertAudioTo16Bit(o)";
                    fileContents += "\r\n\r\n";

                    fileContents += "o";

                    Encode(true, true);
                }

                writeFile(outputFileName, fileContents);

                if (File.Exists(output))
                {
                    container(chbMP4.Checked, chbMKV.Checked);
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

        private void chkVEnc_CheckedChanged(object sender, EventArgs e)
        {
            chbMP4.Checked = chkVEnc.Checked &&
                             chbMP4.Enabled &&
                             cbxAudioCodec.SelectedIndex != (int) Audio.OPUS &&
                             !chbMKV.Checked;

            cbxVideoCodec.Enabled = chkVEnc.Checked;
        }

        private void chkAEnc_CheckedChanged(object sender, EventArgs e)
        {
            cbxAudioCodec.Enabled = chkAEnc.Checked;
        }

        private void cbxAudioCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkVEnc.Enabled)
            {
                if (cbxAudioCodec.SelectedIndex == (int) Audio.OPUS)
                {
                    chbMP4.Enabled = false;
                    chbMP4.Checked = false;
                    chbMKV.Checked = true;
                }
                else chbMP4.Enabled = true;
            }
        }

        private void chbMP4_CheckedChanged(object sender, EventArgs e)
        {
            if (chbMP4.Checked) chbMKV.Checked = false;
        }

        private void chbMKV_CheckedChanged(object sender, EventArgs e)
        {
            if (chbMKV.Checked) chbMP4.Checked = false;
        }
        #endregion ComponentEvents
    }
}
