namespace Simple_AVS_Generator.Modules
{
    internal class SupportedExts
    {
        public string? SupportedContainerExts { get; private set; }
        public string? SupportedVideoExts { get; private set; }
        public string? SupportedAudioExts { get; private set; }

        public string? FilterContainerExts { get; private set; }
        public string? FilterVideoExts { get; private set; }
        public string? FilterAudioExts { get; private set; }

        private object [,] extensions =
        {
            { Enums.ExtensionTypes.CONTAINER, ".3gp"  },
            { Enums.ExtensionTypes.CONTAINER, ".3g2"  },
            { Enums.ExtensionTypes.CONTAINER, ".asf"  },
            { Enums.ExtensionTypes.CONTAINER, ".avi"  },
            { Enums.ExtensionTypes.CONTAINER, ".flv"  },
            { Enums.ExtensionTypes.CONTAINER, ".mp4"  },
            { Enums.ExtensionTypes.CONTAINER, ".m4v"  },
            { Enums.ExtensionTypes.CONTAINER, ".mkv"  },
            { Enums.ExtensionTypes.CONTAINER, ".mov"  },
            { Enums.ExtensionTypes.CONTAINER, ".m2t"  },
            { Enums.ExtensionTypes.CONTAINER, ".m2ts" },
            { Enums.ExtensionTypes.CONTAINER, ".mxf"  },
            { Enums.ExtensionTypes.CONTAINER, ".ogm"  },
            { Enums.ExtensionTypes.CONTAINER, ".rm"   },
            { Enums.ExtensionTypes.CONTAINER, ".rmvb" },
            { Enums.ExtensionTypes.CONTAINER, ".ts"   },
            { Enums.ExtensionTypes.CONTAINER, ".wmv"  },

            { Enums.ExtensionTypes.VIDEO,     ".263"  },
            { Enums.ExtensionTypes.VIDEO,     ".h263" },
            { Enums.ExtensionTypes.VIDEO,     ".264"  },
            { Enums.ExtensionTypes.VIDEO,     ".h264" },
            { Enums.ExtensionTypes.VIDEO,     ".265"  },
            { Enums.ExtensionTypes.VIDEO,     ".h265" },
            { Enums.ExtensionTypes.VIDEO,     ".hevc" },
            { Enums.ExtensionTypes.VIDEO,     ".y4m"  },

            { Enums.ExtensionTypes.AUDIO,     ".aa3"  },
            { Enums.ExtensionTypes.AUDIO,     ".aac"  },
            { Enums.ExtensionTypes.AUDIO,     ".aif"  },
            { Enums.ExtensionTypes.AUDIO,     ".ac3"  },
            { Enums.ExtensionTypes.AUDIO,     ".ape"  },
            { Enums.ExtensionTypes.AUDIO,     ".dts"  },
            { Enums.ExtensionTypes.AUDIO,     ".flac" },
            { Enums.ExtensionTypes.AUDIO,     ".m1a"  },
            { Enums.ExtensionTypes.AUDIO,     ".m2a"  },
            { Enums.ExtensionTypes.AUDIO,     ".mp2"  },
            { Enums.ExtensionTypes.AUDIO,     ".mp3"  },
            { Enums.ExtensionTypes.AUDIO,     ".m4a"  },
            { Enums.ExtensionTypes.AUDIO,     ".oma"  },
            { Enums.ExtensionTypes.AUDIO,     ".opus" },
            { Enums.ExtensionTypes.AUDIO,     ".thd"  },
            { Enums.ExtensionTypes.AUDIO,     ".tta"  },
            { Enums.ExtensionTypes.AUDIO,     ".wav"  },
            { Enums.ExtensionTypes.AUDIO,     ".wma"  }
        };

        public SupportedExts()
        {
            SetSupportedContainerExts();
            SetSupportedVideoExts();
            SetSupportedAudioExts();

            SetFilterContainerExts();
            SetFilterVideoExts();
            SetFilterAudioExts();
        }

        private void SetSupportedContainerExts()
        {
            for (int i = 0; i < extensions.GetLength(0); i++)
            {
                if ((int)extensions[i, 0] == (int)Enums.ExtensionTypes.CONTAINER)
                {
                    SupportedContainerExts += $"*{extensions[i, 1]};";
                }
            }

            SupportedContainerExts = SupportedContainerExts?.Remove(SupportedContainerExts.LastIndexOf(";"));
        }

        private void SetSupportedVideoExts()
        {
            for (int i = 0; i < extensions.GetLength(0); i++)
            {
                if ((int)extensions[i, 0] == (int)Enums.ExtensionTypes.VIDEO)
                {
                    SupportedVideoExts += $"*{extensions[i, 1]};";
                }
            }

            SupportedVideoExts = SupportedVideoExts?.Remove(SupportedVideoExts.LastIndexOf(";"));
        }

        private void SetSupportedAudioExts()
        {
            for (int i = 0; i < extensions.GetLength(0); i++)
            {
                if ((int)extensions[i, 0] == (int)Enums.ExtensionTypes.AUDIO)
                {
                    SupportedAudioExts += $"*{extensions[i, 1]};";
                }
            }

            SupportedAudioExts = SupportedAudioExts?.Remove(SupportedAudioExts.LastIndexOf(";"));
        }

        private void SetFilterContainerExts()
        {
            for (int i = 0; i < extensions.GetLength(0); i++)
            {
                if ((int)extensions[i, 0] == (int)Enums.ExtensionTypes.CONTAINER)
                {
                    FilterContainerExts += $"{extensions[i, 1].ToString()?.Substring(1).ToUpper()} ";
                }
            }

            FilterContainerExts = FilterContainerExts?.Remove(FilterContainerExts.LastIndexOf(" "));
        }

        private void SetFilterVideoExts()
        {
            for (int i = 0; i < extensions.GetLength(0); i++)
            {
                if ((int)extensions[i, 0] == (int)Enums.ExtensionTypes.VIDEO)
                {
                    FilterVideoExts += $"{extensions[i, 1].ToString()?.Substring(1).ToUpper()} ";
                }
            }

            FilterVideoExts = FilterVideoExts?.Remove(FilterVideoExts.LastIndexOf(" "));
        }

        private void SetFilterAudioExts()
        {
            for (int i = 0; i < extensions.GetLength(0); i++)
            {
                if ((int)extensions[i, 0] == (int)Enums.ExtensionTypes.AUDIO)
                {
                    FilterAudioExts += $"{extensions[i, 1].ToString()?.Substring(1).ToUpper()} ";
                }
            }

            FilterAudioExts = FilterAudioExts?.Remove(FilterAudioExts.LastIndexOf(" "));
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
