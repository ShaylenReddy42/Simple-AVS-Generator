using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

/*  Known Bugs And Features
    #01 Program Doesn't Remember Last Position

    #02 Cannot Write File To "C:\\"
        [Crashes: Access Violation]

    #03 Currently No Way Of Manually Detecting
        Codec To Simplify Choice Of Source

   X#04 Implement Codecs:
        OPUS, MP1, MP2

    #05 Implement Help, About And Prerequisites 
 */

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

        String fileName = "", fileDir = "", v = "", a = "", output = "", fileExt = "", fileNameOnly = "",
               outDir = "C:\\Users\\" + Environment.UserName + "\\Desktop\\Temp\\",
               home = "C:\\Users\\" + Environment.UserName + "\\Desktop\\Temp\\";
        //int fps = 0;

        String cS = "*.3gp;*.3g2;*.mp4;*.mkv;*.avi;*.mov;*.m4v;*.flv",
               vS = "*.264;*.265;*.vp9",
               aS = "*.aac;*.m1a;*.m2a;*.mp3;*.m4a;*.dts;*.ac3;*.opus";

        void populateComboLists()
        {
            cbxAudioCodec.Items.Add("AAC-LC")       ;
            cbxAudioCodec.Items.Add("AAC-HE")       ;
            cbxAudioCodec.Items.Add("OPUS")         ;
            cbxAudioCodec.SelectedIndex = 0         ;

            cbxVideoCodec.Items.Add("HEVC")         ;
            cbxVideoCodec.Items.Add("AVC")          ;
            cbxVideoCodec.Items.Add("Whatsapp")     ;
            cbxVideoCodec.Items.Add("Mux Original") ;
            cbxVideoCodec.SelectedIndex = 0         ;

            cbxLanguage.Items.Add("English")        ;
            cbxLanguage.Items.Add("Hindi")          ;
            cbxLanguage.Items.Add("Japanese")       ;
            cbxLanguage.Items.Add("Tamil")          ;
            cbxLanguage.SelectedIndex = 0           ;

            cbxChannels.Items.Add("2 Channels")     ;
            cbxChannels.Items.Add("5.1 Channels")   ;
            cbxChannels.Items.Add("7.1 Channels")   ;
            cbxChannels.SelectedIndex = 0           ;
        }

        int determineAudioBitrate()
        {
            int audioBitrate = 0;

            switch (cbxChannels.SelectedIndex)
            {
                case 0:
                    audioBitrate = cbxAudioCodec.SelectedIndex == 0 ? 112 : cbxAudioCodec.SelectedIndex == 1 ? 80 : 96  ;
                    break;
                case 1:
                    audioBitrate = cbxAudioCodec.SelectedIndex == 0 ? 320 : cbxAudioCodec.SelectedIndex == 1 ? 224 : 288;
                    break;
                case 2:
                    audioBitrate = cbxAudioCodec.SelectedIndex == 0 ? 448 : cbxAudioCodec.SelectedIndex == 1 ? 320 : 384;
                    break;
            }

            return audioBitrate;
        }

        String determineAudioLanguage()
        {
            String audioLanguage = "";

            switch (cbxLanguage.SelectedIndex)
            {
                case 0:
                    audioLanguage = "eng";
                    break;
                case 1:
                    audioLanguage = "hin";
                    break;
                case 2:
                    audioLanguage = "jpn";
                    break;
                case 3:
                    audioLanguage = "tam";
                    break;
            }

            return audioLanguage;
        }

        #region Methods
        void EnableEncodeAndContainer()
        {
            chkVEnc.Enabled     = true;
            chkAEnc.Enabled     = true;
            cbxChannels.Enabled = true;
            cbxLanguage.Enabled = true;
            chbMP4.Enabled      = cbxAudioCodec.SelectedIndex != 1 ? true : false;
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
            String switches = " -i -l";
            StreamWriter sw = new StreamWriter(outDir + "AVSMeter.cmd");
            sw.WriteLine("AVSMeter64 \"" + output + "\"" + switches);
            sw.Close();
            /*String fpsLine = "";
            StreamReader sr = new StreamReader(outDir + "\\Script.log");
            if (sr.ReadLine().Contains("Number of frames:"))
            {
                fpsLine = sr.ReadLine();
            }
            sr.Close();
            fps = (int)fpsLine.LastIndexOf(" ");*/
        }

        void Encode(bool video, bool audio)
        {
            if (video)
            {
                String vPipe    = "avs2pipemod " + "-y4mp \"" + output + "\" | ";
                String vEncoder = "";
                String vCmdFile = outDir;

                if (cbxVideoCodec.SelectedIndex == 0)
                {
                    vEncoder += "x265 -P main --preset slower --crf 27 -i 1 -I 48 --scenecut-bias 10 --bframes 3 ";
                    vEncoder += "--aq-mode 3 --aq-motion --aud --no-open-gop --y4m -f 0 - \"" + outDir + "Video.265\"";
                    vCmdFile += "Encode Video [HEVC].cmd";
                }
                else if (cbxVideoCodec.SelectedIndex == 1)
                {
                    vEncoder += "x264 --preset slower --crf 27 -i 1 -I 48 --bframes 3 --aq-mode 3 --aud --no-mbtree ";
                    vEncoder += "--demuxer y4m --frames 0 -o \"" + outDir + "Video.264\" -";
                    vCmdFile += "Encode Video [AVC].cmd";
                }
                else if (cbxVideoCodec.SelectedIndex == 2)
                {
                    vEncoder += "x264 --preset slower --crf 27 -i 1 -I 10000 --bframes 1 --no-scenecut --aud --no-mbtree ";
                    vEncoder += "--demuxer y4m --frames 0 -o \"" + outDir + "Video.264\" -";
                    vCmdFile += "Encode Video [AVC].cmd";
                }
                
                StreamWriter sw = new StreamWriter(vCmdFile);
                sw.WriteLine(vPipe + vEncoder);
                sw.Close();
                //avsMeter();
            }

            if (audio)
            {
                String aPipe    = "avs2pipemod " + "-wav=16bit \"" + output + "\" | ";
                String aEncoder = "";
                String aCmdFile = outDir;

                if (cbxAudioCodec.SelectedIndex == 0)
                {
                    aEncoder += "qaac64 --abr " + determineAudioBitrate() + " --ignorelength --no-delay ";
                    aEncoder += "-o \"" + outDir + fileNameOnly + ".m4a\" - ";
                    aCmdFile += "Encode Audio [AAC-LC].cmd";
                }
                else if (cbxAudioCodec.SelectedIndex == 1)
                {
                    aEncoder += "qaac64 --he --abr " + determineAudioBitrate() + " --ignorelength ";
                    aEncoder += "-o \"" + outDir + fileNameOnly + ".m4a\" - ";
                    aCmdFile += "Encode Audio [AAC-HE].cmd";
                }
                else
                {
                    aEncoder += "opusenc --bitrate " + determineAudioBitrate() + " --ignorelength ";
                    aEncoder += "- \"" + outDir + fileNameOnly + ".ogg\"";
                    aCmdFile += "Encode Audio [OPUS].cmd";
                }

                StreamWriter sw = new StreamWriter(aCmdFile);
                sw.WriteLine(aPipe + aEncoder);
                sw.Close();
            }
        }

        void container(bool mp4, bool mkv)
        {
            String videoExtension = cbxVideoCodec.SelectedIndex == 0 ? ".265" : ".264";
            String audioExtension = cbxAudioCodec.SelectedIndex == 2 ? ".ogg" : ".m4a";

            if (mp4)
            {
                String mp4V = "-add \"" + outDir + "Video" + videoExtension + "\":name= ";
                String mp4A = chkAEnc.Checked == true ?  "-add \"" + outDir + fileNameOnly + ".m4a\":name=:lang=" + determineAudioLanguage() : "";
                String newmp4 = " -new " + "\"" + outDir + fileNameOnly + ".mp4\"";
                StreamWriter sw = new StreamWriter(outDir + "MP4 Mux.cmd");
                sw.WriteLine("mp4box " + mp4V + mp4A + newmp4);
                sw.Close();
            }

            if (mkv)
            {
                String mkvO = "-o \"" + outDir + fileNameOnly + ".mkv\" ";
                String mkvV = "\"" + outDir + "Video" + videoExtension + "\" ";
                String mkvA = chkAEnc.Checked == true ? "--language 0:eng \"" + outDir + fileNameOnly + audioExtension + "\" " : "";
                StreamWriter sw = new StreamWriter(outDir + "MKV Mux.cmd");
                sw.WriteLine("mkvmerge " + mkvO + mkvV + mkvA);
                sw.Close();
            }

            if (cbxVideoCodec.SelectedIndex == 3)
            {
                String mp4V = "-add \"" + fileName + "\"#video ";
                String mp4A = chkAEnc.Checked == true ? "-add \"" + outDir + fileNameOnly + ".m4a\":name=:lang=" + determineAudioLanguage() : "";
                String newmp4 = " -new " + "\"" + outDir + fileNameOnly + ".mp4\"";
                StreamWriter sw = new StreamWriter(outDir + "MP4 Mux [Original Video].cmd");
                sw.WriteLine("mp4box " + mp4V + mp4A + newmp4);
                sw.Close();
            }
        }
        #endregion Methods

        #region Buttons
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            String sF = "All Supported|" + cS + ";" + vS + ";" + aS;
            String cF = "Container Types [3GP 3G2 AVI FLV MP4 MKV MOV M4V]|" + cS;
            String vF = "Video Types [264 265 VP9]|" + vS;
            String aF = "Audio Types [AAC AC3 DTS M1A M2A MP3 M4A]|" + aS;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Title = "Open File";
            ofd.Filter = sF + "|" + cF + "|" + vF + "|" + aF;

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
                v = chkVEnc.Checked & cbxVideoCodec.SelectedIndex != 3 ? "v=LWLibavVideoSource(i).ConvertToYV12()" : "";
                v = cbxVideoCodec.SelectedIndex == 2 ? v + ".Spline36Resize(480, 270)" : v;

                a = chkAEnc.Checked ? "a=LWLibavAudioSource(i).ConvertAudioTo16Bit()" : "";

                StreamWriter sw = null;

                if (v == "" && a != "")
                {
                    sw = new StreamWriter(output);

                    sw.WriteLine(i);
                    sw.WriteLine("");

                    sw.WriteLine(a);
                    sw.WriteLine("");
                    sw.WriteLine("a=Normalize(a, 1.0)");
                    sw.WriteLine("");
                    sw.WriteLine("a");

                    Encode(false, true);
                }
                else if (a == "" && v != "")
                {
                    sw = new StreamWriter(output);

                    sw.WriteLine(i);
                    sw.WriteLine("");

                    sw.WriteLine(v);
                    sw.WriteLine("");
                    sw.WriteLine("v");

                    Encode(true, false);
                }
                else if (v != "" && a != "")
                {
                    sw = new StreamWriter(output);

                    sw.WriteLine(i);
                    sw.WriteLine("");

                    sw.WriteLine(v);
                    sw.WriteLine("");

                    sw.WriteLine(a);
                    sw.WriteLine("");
                    sw.WriteLine("a=Normalize(a, 1.0)");
                    sw.WriteLine("");

                    sw.WriteLine("o=AudioDub(v, a)");
                    sw.WriteLine("");
                    sw.WriteLine("o");

                    Encode(true, true);
                }

                if (File.Exists(output))
                {
                    sw.Close();
                    container(chbMP4.Checked ? true : false, chbMKV.Checked ? true : false);
                    New();
                }
            }
            else MessageBox.Show("Please Input A File First");
        }
        
        private void btnNew_Click(object sender, EventArgs e)
        {
            New();
        }
        #endregion Buttons

        #region Container
        private void chbMP4_CheckedChanged(object sender, EventArgs e)
        {
            if (chbMP4.Checked) chbMKV.Checked = false;
        }

        private void chbMKV_CheckedChanged(object sender, EventArgs e)
        {
            if (chbMKV.Checked) chbMP4.Checked = false;
        }
        #endregion Container

        #region ComponentEvents
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileName != "" && Directory.Exists(outDir) && !File.Exists(output))
                Directory.Delete(outDir);
        }

        private void chkVEnc_CheckedChanged(object sender, EventArgs e)
        {
            chbMP4.Checked          = chkVEnc.Checked && cbxAudioCodec.SelectedIndex != 1 ? true : false;
            cbxVideoCodec.Enabled   = chkVEnc.Checked ? true : false;
        }

        private void chkAEnc_CheckedChanged(object sender, EventArgs e)
        {
            cbxAudioCodec.Enabled   = chkAEnc.Checked ? true : false;
        }
        
        private void cbxVideoCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxVideoCodec.SelectedIndex == 3)
            {
                chbMP4.Checked = false;
            }
        }

        private void cbxAudioCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkVEnc.Enabled)
            {
                if (cbxAudioCodec.SelectedIndex == 2)
                {
                    chbMP4.Enabled  = false;
                    chbMP4.Checked  = false;
                    chbMKV.Checked  = true;
                }
                else chbMP4.Enabled = true;
            }
        }
        #endregion ComponentEvents
    }
}
