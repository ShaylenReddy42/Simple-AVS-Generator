/******************************************************************************
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

using static SimpleAVSGenerator.Core.Enums;

namespace SimpleAVSGenerator.Core.Support
{
    public class Extensions
    {
        public string? SupportedContainerExts { get; private set; }
        public string? SupportedVideoExts { get; private set; }
        public string? SupportedAudioExts { get; private set; }

        public string? FilterContainerExts { get; private set; }
        public string? FilterVideoExts { get; private set; }
        public string? FilterAudioExts { get; private set; }

        // ExtensionType | FileExtension | IsSupportedByMP4Box
        private object[,] extensions =
        {
            { ExtensionTypes.CONTAINER, ".3gp",  true  },
            { ExtensionTypes.CONTAINER, ".3g2",  true  },
            { ExtensionTypes.CONTAINER, ".asf",  false },
            { ExtensionTypes.CONTAINER, ".avi",  true  },
            { ExtensionTypes.CONTAINER, ".flv",  false },
            { ExtensionTypes.CONTAINER, ".mp4",  true  },
            { ExtensionTypes.CONTAINER, ".m4v",  true  },
            { ExtensionTypes.CONTAINER, ".mkv",  false },
            { ExtensionTypes.CONTAINER, ".mov",  false },
            { ExtensionTypes.CONTAINER, ".m2t",  true  },
            { ExtensionTypes.CONTAINER, ".m2ts", true  },
            { ExtensionTypes.CONTAINER, ".mxf",  false },
            { ExtensionTypes.CONTAINER, ".ogm",  false },
            { ExtensionTypes.CONTAINER, ".rm",   false },
            { ExtensionTypes.CONTAINER, ".rmvb", false },
            { ExtensionTypes.CONTAINER, ".ts",   true  },
            { ExtensionTypes.CONTAINER, ".wmv",  false },

            { ExtensionTypes.VIDEO,     ".263",  true  },
            { ExtensionTypes.VIDEO,     ".h263", true  },
            { ExtensionTypes.VIDEO,     ".264",  true  },
            { ExtensionTypes.VIDEO,     ".h264", true  },
            { ExtensionTypes.VIDEO,     ".265",  true  },
            { ExtensionTypes.VIDEO,     ".h265", true  },
            { ExtensionTypes.VIDEO,     ".hevc", true  },
            { ExtensionTypes.VIDEO,     ".y4m",  false },

            { ExtensionTypes.AUDIO,     ".aa3",  false },
            { ExtensionTypes.AUDIO,     ".aac",  true  },
            { ExtensionTypes.AUDIO,     ".aif",  false },
            { ExtensionTypes.AUDIO,     ".ac3",  true  },
            { ExtensionTypes.AUDIO,     ".ape",  false },
            { ExtensionTypes.AUDIO,     ".dts",  false },
            { ExtensionTypes.AUDIO,     ".flac", true  },
            { ExtensionTypes.AUDIO,     ".m1a",  true  },
            { ExtensionTypes.AUDIO,     ".m2a",  true  },
            { ExtensionTypes.AUDIO,     ".mp2",  true  },
            { ExtensionTypes.AUDIO,     ".mp3",  true  },
            { ExtensionTypes.AUDIO,     ".m4a",  true  },
            { ExtensionTypes.AUDIO,     ".oma",  false },
            { ExtensionTypes.AUDIO,     ".opus", true  },
            { ExtensionTypes.AUDIO,     ".thd",  false },
            { ExtensionTypes.AUDIO,     ".tta",  false },
            { ExtensionTypes.AUDIO,     ".wav",  true  },
            { ExtensionTypes.AUDIO,     ".wma",  false }
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

        public bool IsSupportedByMP4Box(string fileExt)
        {
            bool supported = default;

            for (int i = 0; i < extensions.GetLength(0); i++)
            {
                if (fileExt.Equals((string)extensions[i, 1], StringComparison.OrdinalIgnoreCase))
                {
                    supported = (bool)extensions[i, 2];
                }
            }

            return supported;
        }
    }
}
