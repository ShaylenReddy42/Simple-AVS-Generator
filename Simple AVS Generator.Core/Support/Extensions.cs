using static Simple_AVS_Generator.Core.Enums;

namespace Simple_AVS_Generator.Core.Support
{
    public class Extensions
    {
        public string? SupportedContainerExts { get; private set; }
        public string? SupportedVideoExts { get; private set; }
        public string? SupportedAudioExts { get; private set; }

        public string? FilterContainerExts { get; private set; }
        public string? FilterVideoExts { get; private set; }
        public string? FilterAudioExts { get; private set; }

        private object [,] extensions =
        {
            { ExtensionTypes.CONTAINER, ".3gp"  },
            { ExtensionTypes.CONTAINER, ".3g2"  },
            { ExtensionTypes.CONTAINER, ".asf"  },
            { ExtensionTypes.CONTAINER, ".avi"  },
            { ExtensionTypes.CONTAINER, ".flv"  },
            { ExtensionTypes.CONTAINER, ".mp4"  },
            { ExtensionTypes.CONTAINER, ".m4v"  },
            { ExtensionTypes.CONTAINER, ".mkv"  },
            { ExtensionTypes.CONTAINER, ".mov"  },
            { ExtensionTypes.CONTAINER, ".m2t"  },
            { ExtensionTypes.CONTAINER, ".m2ts" },
            { ExtensionTypes.CONTAINER, ".mxf"  },
            { ExtensionTypes.CONTAINER, ".ogm"  },
            { ExtensionTypes.CONTAINER, ".rm"   },
            { ExtensionTypes.CONTAINER, ".rmvb" },
            { ExtensionTypes.CONTAINER, ".ts"   },
            { ExtensionTypes.CONTAINER, ".wmv"  },

            { ExtensionTypes.VIDEO,     ".263"  },
            { ExtensionTypes.VIDEO,     ".h263" },
            { ExtensionTypes.VIDEO,     ".264"  },
            { ExtensionTypes.VIDEO,     ".h264" },
            { ExtensionTypes.VIDEO,     ".265"  },
            { ExtensionTypes.VIDEO,     ".h265" },
            { ExtensionTypes.VIDEO,     ".hevc" },
            { ExtensionTypes.VIDEO,     ".y4m"  },

            { ExtensionTypes.AUDIO,     ".aa3"  },
            { ExtensionTypes.AUDIO,     ".aac"  },
            { ExtensionTypes.AUDIO,     ".aif"  },
            { ExtensionTypes.AUDIO,     ".ac3"  },
            { ExtensionTypes.AUDIO,     ".ape"  },
            { ExtensionTypes.AUDIO,     ".dts"  },
            { ExtensionTypes.AUDIO,     ".flac" },
            { ExtensionTypes.AUDIO,     ".m1a"  },
            { ExtensionTypes.AUDIO,     ".m2a"  },
            { ExtensionTypes.AUDIO,     ".mp2"  },
            { ExtensionTypes.AUDIO,     ".mp3"  },
            { ExtensionTypes.AUDIO,     ".m4a"  },
            { ExtensionTypes.AUDIO,     ".oma"  },
            { ExtensionTypes.AUDIO,     ".opus" },
            { ExtensionTypes.AUDIO,     ".thd"  },
            { ExtensionTypes.AUDIO,     ".tta"  },
            { ExtensionTypes.AUDIO,     ".wav"  },
            { ExtensionTypes.AUDIO,     ".wma"  }
        };

        public Extensions()
        {
            SetSupportFor((int)ExtensionTypes.CONTAINER);
            SetSupportFor((int)ExtensionTypes.VIDEO);
            SetSupportFor((int)ExtensionTypes.AUDIO);

            SetFilterFor((int)ExtensionTypes.CONTAINER);
            SetFilterFor((int)ExtensionTypes.VIDEO);
            SetFilterFor((int)ExtensionTypes.AUDIO);
        }

        private void SetSupportFor(int fileType)
        {
            string support = "";

            for (int i = 0; i < extensions.GetLength(0); i++)
            {
                if ((int)extensions[i, 0] == fileType)
                {
                    support += $"*{extensions[i, 1]};";
                }
            }

            support = support.Remove(support.LastIndexOf(";"));

            switch (fileType)
            {
                case (int)ExtensionTypes.CONTAINER:
                    SupportedContainerExts = support;
                    break;
                case (int)ExtensionTypes.VIDEO:
                    SupportedVideoExts = support;
                    break;
                case (int)ExtensionTypes.AUDIO:
                    SupportedAudioExts = support;
                    break;
            }
        }

        private void SetFilterFor(int fileType)
        {
            string filter = "";
            
            for (int i = 0; i < extensions.GetLength(0); i++)
            {
                if ((int)extensions[i, 0] == fileType)
                {
                    filter += $"{extensions[i, 1].ToString()?.Substring(1).ToUpper()} ";
                }
            }

            filter = filter.Remove(filter.LastIndexOf(" "));

            switch (fileType)
            {
                case (int)ExtensionTypes.CONTAINER:
                    FilterContainerExts = filter;
                    break;
                case (int)ExtensionTypes.VIDEO:
                    FilterVideoExts = filter;
                    break;
                case (int)ExtensionTypes.AUDIO:
                    FilterAudioExts = filter;
                    break;
            }
        }

        public int DetermineInputFileType(string fileExt)
        {
            int ext = default;
            
            for (int i = 0; i < extensions.GetLength(0); i++)
            {
                if (fileExt.Equals((string)extensions[i,1], StringComparison.OrdinalIgnoreCase))
                {
                    ext = (int)extensions[i,0];
                }
            }

            return ext;
        }
    }
}
