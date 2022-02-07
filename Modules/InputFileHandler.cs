using static Simple_AVS_Generator.Modules.Common;

namespace Simple_AVS_Generator.Modules
{
    internal class InputFileHandler
    {
        public string FileName { get; private set; }
        public string FileExt { get; private set; }
        public string FileNameOnly { get; private set; }
        public int FileType { get; private set; }

        public string? OutputDir { get; set; }

        public bool Video { private get; set; }
        public bool Audio { private get; set; }

        private bool isSupportedByMP4Box = default;

        private string? AVSMeterScriptFile => $"{OutputDir}AVSMeter.cmd";
        private string? AVSMeterScriptContents => $"AVSMeter64 \"%~dp0Script.avs\" -i -l";

        //Video Properties
        public int VideoCodec { private get; set; }
        public int SourceFPS { private get; set; }
        public int KeyframeIntervalInSeconds { private get; set; }
        public bool NeedsToBeResized { private get; set; }

        //Audio Properties
        public int AudioBitrate { private get; set; }
        public string? AudioLanguage { private get; set; }

        public int? OutputContainer { private get; set; }
        public bool MuxOriginalVideo { private get; set; }

        public AviSynthScript? Script = null;
        public OutputScripts? OutputScripts = null;
        
        public InputFileHandler(string fileName)
        {
            FileName = fileName;
            FileExt = Path.GetExtension(FileName);
            FileNameOnly = Path.GetFileNameWithoutExtension(FileName);

            SupportedExts se = new();
            FileType = se.DetermineInputFileType(FileExt);

            Script = new(FileName);
            OutputScripts = new();
        }

        public bool IsSupportedByMP4Box
        {
            get => isSupportedByMP4Box;
            private set
            {
                string[] supportedExts =
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

                foreach (string ext in supportedExts)
                {
                    if (FileExt.Equals(ext, StringComparison.CurrentCultureIgnoreCase))
                    {
                        IsSupportedByMP4Box = true;
                        break;
                    }
                }
            }
        }

        public void CreateScripts()
        {
            Script.Video = Video;
            Script.Audio = Audio;
            Script.NeedsToBeResized = NeedsToBeResized;
            Script.SetScriptContent();
            WriteFile(Script.ScriptFile, Script.ScriptContent);

            WriteFile(AVSMeterScriptFile, AVSMeterScriptContents);

            OutputScripts.OutputDir = OutputDir;
            OutputScripts.FileName = FileName;
            OutputScripts.FileNameOnly = FileNameOnly;

            OutputScripts.Video = Video;
            OutputScripts.VideoCodec = VideoCodec;
            OutputScripts.SourceFPS = SourceFPS;
            OutputScripts.KeyframeIntervalInSeconds = KeyframeIntervalInSeconds;

            OutputScripts.ConfigureVideoScript();
            if (OutputScripts.VideoEncoderScriptContents is not null)
            {
                WriteFile(OutputScripts.VideoEncoderScriptFile, OutputScripts.VideoEncoderScriptContents);
            }

            OutputScripts.Audio = Audio;
            OutputScripts.AudioBitrate = AudioBitrate;
            OutputScripts.AudioLanguage = AudioLanguage;

            OutputScripts.ConfigureAudioScript();
            if (OutputScripts.AudioEncoderScriptContents is not null)
            {
                WriteFile(OutputScripts.AudioEncoderScriptFile, OutputScripts.AudioEncoderScriptContents);
            }

            OutputScripts.OutputContainer = OutputContainer;
            OutputScripts.ConfigureContainerScript(MuxOriginalVideo);
            if (OutputScripts.ContainerScriptContents is not null)
            {
                WriteFile(OutputScripts.ContainerScriptFile, OutputScripts.ContainerScriptContents);
            }
        }
    }
}
