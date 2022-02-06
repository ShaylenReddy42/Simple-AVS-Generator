using static Simple_AVS_Generator.Modules.Enums;

namespace Simple_AVS_Generator.Modules
{
    internal class OutputScripts
    {
        public string? OutputDir { private get; set; }
        public string? FileName { private get; set;}
        public string? FileNameOnly { private get; set; }
        
        public bool Video { private get; set; }
        public int VideoCodec { private get; set; }
        public int SourceFPS { private get; set; }
        public int KeyframeIntervalInSeconds { private get; set; }

        public string? VideoEncoderScriptFile { get; private set; }
        public string? VideoEncoderScriptContents { get; private set; }

        public bool Audio { private get; set; }
        public int AudioCodec { private get; set; }
        public int AudioBitrate { private get; set; }
        public string? AudioLanguage { private get; set; }

        public string? AudioEncoderScriptFile { get; private set; }
        public string? AudioEncoderScriptContents { get; private set; }

        public int? OutputContainer { private get; set; }
        public string? ContainerScriptFile { get; private set; }
        public string? ContainerScriptContents { get; private set; }

        public OutputScripts()
        {
            Video = default;
            Audio = default;
        }

        private int GetKeyframeIntervalInFrames() { return SourceFPS * KeyframeIntervalInSeconds; }

        public void ConfigureVideoScript()
        {
            if (Video)
            {
                string vPipe = "avs2pipemod -y4mp \"%~dp0Script.avs\" | ",
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
                VideoEncoderScriptContents = vPipe + vEncoder;
            }
        }

        public void ConfigureAudioScript()
        {
            if (Audio)
            {
                string aPipe = "avs2pipemod -wav=16bit \"%~dp0Script.avs\" | ",
                       aEncoder = "",
                       aCmdFile = OutputDir;

                if (AudioCodec == (int)AudioCodecs.AAC_LC)
                {
                    aEncoder += $"qaac64 --abr {AudioBitrate} --ignorelength --no-delay ";
                    aEncoder += $"-o \"%~dp0{FileNameOnly}.m4a\" - ";
                    aCmdFile += "Encode Audio [AAC-LC].cmd";
                }
                else if (AudioCodec == (int)AudioCodecs.AAC_HE)
                {
                    aEncoder += $"qaac64 --he --abr {AudioBitrate} --ignorelength ";
                    aEncoder += $"-o \"%~dp0{FileNameOnly}.m4a\" - ";
                    aCmdFile += "Encode Audio [AAC-HE].cmd";
                }
                else //OPUS
                {
                    aEncoder += $"opusenc --bitrate {AudioBitrate} --ignorelength ";
                    aEncoder += $"- \"%~dp0{FileNameOnly}.ogg\"";
                    aCmdFile += "Encode Audio [OPUS].cmd";
                }

                AudioEncoderScriptFile = aCmdFile;
                AudioEncoderScriptContents = aPipe + aEncoder;
            }
        }

        public void ConfigureContainerScript(bool originalVideo)
        {
            string videoExtension = VideoCodec == (int)VideoCodecs.HEVC ? ".265"
                                  : VideoCodec == (int)VideoCodecs.AV1 ? ".ivf"
                                  : ".264",
                   audioExtension = AudioCodec == (int)AudioCodecs.OPUS ? ".ogg" : ".m4a",

                   outputFileName = OutputDir,
                   fileContents = "";

            if (OutputContainer == (int)OutputContainers.MP4)
            {
                string mp4V = !originalVideo
                              ? $"-add \"%~dp0Video{videoExtension}\":name="
                              : $"-add \"{FileName}\"#video",
                       mp4A = Audio ? $"-add \"%~dp0{FileNameOnly}{audioExtension}\":name=:lang={AudioLanguage}" : "",
                       newmp4 = $"-new \"%~dp0{FileNameOnly}.mp4\"";

                outputFileName += $"MP4 Mux{(originalVideo ? " [Original Video]" : "")}.cmd";
                fileContents = $"mp4box {mp4V} {mp4A} {newmp4}";
            }
            else if (OutputContainer == (int)OutputContainers.MKV)
            {
                string mkvO = $"-o \"%~dp0{FileNameOnly}.mkv\"",
                       mkvV = !originalVideo
                            ? $"\"%~dp0Video{videoExtension}\""
                            : $"--no-audio \"{FileName}\"",
                       mkvA = Audio ? $"--language 0:{AudioLanguage} \"%~dp0{FileNameOnly}{audioExtension}\"" : "";

                outputFileName += $"MKV Mux{(originalVideo ? " [Original Video]" : "")}.cmd";
                fileContents = $"mkvmerge {mkvO} {mkvV} {mkvA}";
            }

            ContainerScriptFile = outputFileName;
            ContainerScriptContents = fileContents;
        }
    }
}
