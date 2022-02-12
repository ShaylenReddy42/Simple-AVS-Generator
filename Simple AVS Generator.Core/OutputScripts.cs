﻿/******************************************************************************
 * Copyright (C) 2022 Shaylen Reddy
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

using static Simple_AVS_Generator.Core.Enums;

namespace Simple_AVS_Generator.Core
{
    internal class OutputScripts
    {
        private string FileName { get; set;}
        private string FileNameOnly { get; set; }
        private string OutputDir { get; set; }

        private bool Video { get; set; }
        private int VideoCodec { get; set; }
        private int SourceFPS { get; set; }
        private int KeyframeIntervalInSeconds { get; set; }
        private bool MuxOriginalVideo { get; set; }
        private string? VideoExtension { get; set; } = null;

        private bool Audio { get; set; }
        private int AudioCodec { get; set; }
        private int AudioBitrate { get; set; }
        private string AudioLanguage { get; set; }
        private string AudioExtension { get; set; }

        private int? OutputContainer { get; set; }
        
        public string? VideoEncoderScriptFile { get; private set; }
        public string? VideoEncoderScriptContent { get; private set; }

        public string? AudioEncoderScriptFile { get; private set; }
        public string? AudioEncoderScriptContent { get; private set; }

        public string? ContainerScriptFile { get; private set; }
        public string? ContainerScriptContent { get; private set; }

        public OutputScripts(Common common)
        {
            OutputDir = common.OutputDir;
            FileName = common.FileName;
            FileNameOnly = common.FileNameOnly;
            
            Video = common.Video;
            VideoCodec = common.VideoCodec;
            SourceFPS = common.SourceFPS;
            KeyframeIntervalInSeconds = common.KeyframeIntervalInSeconds;
            MuxOriginalVideo = common.MuxOriginalVideo;
            VideoExtension = common.VideoExtention;

            Audio = common.Audio;
            AudioCodec = common.AudioCodec;
            AudioBitrate = common.AudioBitrate;
            AudioLanguage = common.AudioLanguage;
            AudioExtension = common.AudioExtension;

            OutputContainer = common.OutputContainer;
        }

        private int GetKeyframeIntervalInFrames() { return SourceFPS * KeyframeIntervalInSeconds; }

        public void ConfigureVideoScript()
        {
            if (Video is true && MuxOriginalVideo is false)
            {
                string? vPipe = "avs2pipemod -y4mp \"%~dp0Script.avs\" | ",
                        vEncoder = "",
                        vCmdFile = OutputDir;

                if (VideoCodec == (int)VideoCodecs.HEVC)
                {
                    vEncoder += $"x265 --profile main --preset slower --crf 26 -i 1 -I {GetKeyframeIntervalInFrames()} --hist-scenecut --hist-threshold 0.02 ";
                    vEncoder += "--fades --aq-mode 4 --aq-motion --aud --no-open-gop --y4m -f 0 - \"%~dp0Video.265\"";
                    vCmdFile += "Encode Video [HEVC].cmd";
                }
                else if (VideoCodec == (int)VideoCodecs.AV1)
                {
                    vEncoder += "aomenc --passes=1 --end-usage=q --cq-level=32 --target-bitrate=0 ";
                    vEncoder += $"--enable-fwd-kf=1 --kf-max-dist={GetKeyframeIntervalInFrames()} --verbose --ivf -o \"%~dp0Video.ivf\" -";
                    vCmdFile += "Encode Video [AV1].cmd";
                }
                else if (VideoCodec == (int)VideoCodecs.AVC)
                {
                    vEncoder += $"x264 --preset veryslow --crf 26 -i 1 -I {GetKeyframeIntervalInFrames()} --bframes 3 --deblock -2:-1 --aq-mode 3 ";
                    vEncoder += "--aud --no-mbtree --demuxer y4m --frames 0 -o \"%~dp0Video.264\" -";
                    vCmdFile += "Encode Video [AVC].cmd";
                }
                else if (VideoCodec == (int)VideoCodecs.WhatsApp)
                {
                    vEncoder += "x264 --profile baseline --preset veryslow --crf 26 -i 1 --ref 1 --deblock -2:-1 ";
                    vEncoder += "--aud --no-mbtree --demuxer y4m --frames 0 -o \"%~dp0Video.264\" -";
                    vCmdFile += "Encode Video [AVC].cmd";
                }

                VideoEncoderScriptFile = vCmdFile;
                VideoEncoderScriptContent = vPipe + vEncoder;
            }
        }

        public void ConfigureAudioScript()
        {
            if (Audio)
            {
                string? aPipe = "avs2pipemod -wav=16bit \"%~dp0Script.avs\" | ",
                        aEncoder = "",
                        aCmdFile = OutputDir;

                if (AudioCodec == (int)AudioCodecs.AAC_LC)
                {
                    aEncoder += $"qaac64 --abr {AudioBitrate} --ignorelength --no-delay ";
                    aEncoder += $"-o \"%~dp0{FileNameOnly}{AudioExtension}\" - ";
                    aCmdFile += "Encode Audio [AAC-LC].cmd";
                }
                else if (AudioCodec == (int)AudioCodecs.AAC_HE)
                {
                    aEncoder += $"qaac64 --he --abr {AudioBitrate} --ignorelength ";
                    aEncoder += $"-o \"%~dp0{FileNameOnly}{AudioExtension}\" - ";
                    aCmdFile += "Encode Audio [AAC-HE].cmd";
                }
                else //OPUS
                {
                    aEncoder += $"opusenc --bitrate {AudioBitrate} --ignorelength ";
                    aEncoder += $"- \"%~dp0{FileNameOnly}{AudioExtension}\"";
                    aCmdFile += "Encode Audio [OPUS].cmd";
                }

                AudioEncoderScriptFile = aCmdFile;
                AudioEncoderScriptContent = aPipe + aEncoder;
            }
        }

        public void ConfigureContainerScript()
        {
            string? outputFileName = OutputDir,
                    fileContents = null;

            if (OutputContainer == (int)OutputContainers.MP4)
            {
                string mp4V = MuxOriginalVideo is false
                            ? $"-add \"%~dp0Video{VideoExtension}\":name="
                            : $"-add \"{FileName}\"#video",
                       mp4A = Audio ? $"-add \"%~dp0{FileNameOnly}{AudioExtension}\":name=:lang={AudioLanguage}" : "",
                       newmp4 = $"-new \"%~dp0{FileNameOnly}.mp4\"";

                outputFileName += $"MP4 Mux{(MuxOriginalVideo ? " [Original Video]" : "")}.cmd";
                fileContents = $"mp4box {mp4V} {mp4A} {newmp4}";
            }
            else if (OutputContainer == (int)OutputContainers.MKV)
            {
                string mkvO = $"-o \"%~dp0{FileNameOnly}.mkv\"",
                       mkvV = MuxOriginalVideo is false
                            ? $"\"%~dp0Video{VideoExtension}\""
                            : $"--no-audio \"{FileName}\"",
                       mkvA = Audio ? $"--language 0:{AudioLanguage} \"%~dp0{FileNameOnly}{AudioExtension}\"" : "";

                outputFileName += $"MKV Mux{(MuxOriginalVideo ? " [Original Video]" : "")}.cmd";
                fileContents = $"mkvmerge {mkvO} {mkvV} {mkvA}";
            }

            ContainerScriptFile = outputFileName;
            ContainerScriptContent = fileContents;
        }
    }
}