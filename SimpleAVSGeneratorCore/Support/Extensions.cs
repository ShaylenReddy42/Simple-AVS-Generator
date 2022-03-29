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

namespace SimpleAVSGeneratorCore.Support;

public class Extensions
{
    public string? SupportedContainerExts { get; private set; }
    public string? SupportedVideoExts { get; private set; }
    public string? SupportedAudioExts { get; private set; }

    public string? FilterContainerExts { get; private set; }
    public string? FilterVideoExts { get; private set; }
    public string? FilterAudioExts { get; private set; }

    private static readonly Dictionary<string, Dictionary<string, object>> supportedExtensionsDictionary =
    new()
    {
        /* -------- CONTAINER -------- */
        {
            ".3gp",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", true        }
            }
        },
        {
            ".3g2",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", true        }
            }
        },
        {
            ".asf",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", false       }
            }
        },
        {
            ".avi",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", true        }
            }
        },
        {
            ".flv",
            new()
            {
                { "Type",         "CONTAINER" },
                { "MP4BoxSupport", false      }
            }
        },
        {
            ".mp4",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", true        }
            }
        },
        {
            ".m4v",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", true        }
            }
        },
        {
            ".mkv",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", false       }
            }
        },
        {
            ".mov",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", false       }
            }
        },
        {
            ".m2t",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", true        }
            }
        },
        {
            ".m2ts",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", true        }
            }
        },
        {
            ".mxf",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", false       }
            }
        },
        {
            ".ogm",
            new()
            {
                { "Type",         "CONTAINER" },
                { "MP4BoxSupport", false      }
            }
        },
        {
            ".rm",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", false       }
            }
        },
        {
            ".rmvb",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", false       }
            }
        },
        {
            ".ts",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", true        }
            }
        },
        {
            ".webm",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", false       }
            }
        },
        {
            ".wmv",
            new()
            {
                { "Type",          "CONTAINER" },
                { "MP4BoxSupport", false       }
            }
        },
        /* ---------- VIDEO ---------- */
        {
            ".263",
            new()
            {
                { "Type",          "VIDEO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".h263",
            new()
            {
                { "Type",          "VIDEO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".264",
            new()
            {
                { "Type",          "VIDEO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".h264",
            new()
            {
                { "Type",          "VIDEO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".265",
            new()
            {
                { "Type",          "VIDEO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".h265",
            new()
            {
                { "Type",          "VIDEO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".hevc",
            new()
            {
                { "Type",          "VIDEO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".ivf",
            new()
            {
                { "Type",          "VIDEO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".obu",
            new()
            {
                { "Type",          "VIDEO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".y4m",
            new()
            {
                { "Type",          "VIDEO" },
                { "MP4BoxSupport", false   }
            }
        },
        /* ---------- AUDIO ---------- */
        {
            ".aa3",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", false   }
            }
        },
        {
            ".aac",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".aif",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", false   }
            }
        },
        {
            ".ac3",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".ape",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", false   }
            }
        },
        {
            ".dts",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", false   }
            }
        },
        {
            ".flac",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".m1a",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".m2a",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".mp2",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".mp3",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".m4a",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".oma",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", false   }
            }
        },
        {
            ".opus",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".thd",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", false   }
            }
        },
        {
            ".tta",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", false   }
            }
        },
        {
            ".wav",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", true    }
            }
        },
        {
            ".wma",
            new()
            {
                { "Type",          "AUDIO" },
                { "MP4BoxSupport", false   }
            }
        }
    };

    public Extensions()
    {
        SetSupportFor("CONTAINER");
        SetSupportFor("VIDEO");
        SetSupportFor("AUDIO");

        SetFilterFor("CONTAINER");
        SetFilterFor("VIDEO");
        SetFilterFor("AUDIO");
    }

    private void SetSupportFor(string fileType)
    {
        string support = string.Empty;

        object[] supportedExtensionsDictionaryKeys = supportedExtensionsDictionary.Keys.ToArray();

        foreach (string extension in supportedExtensionsDictionaryKeys)
        {
            Dictionary<string, object> extensionDictionary = supportedExtensionsDictionary[extension];
            if ((string)extensionDictionary["Type"] == fileType)
            {
                support += $"*{extension};";
            }
        }

        support = support.Remove(support.LastIndexOf(";"));

        switch (fileType)
        {
            case "CONTAINER":
                SupportedContainerExts = support;
                break;
            case "VIDEO":
                SupportedVideoExts = support;
                break;
            case "AUDIO":
                SupportedAudioExts = support;
                break;
        }
    }

    private void SetFilterFor(string fileType)
    {
        string filter = string.Empty;

        object[] supportedExtensionsDictionaryKeys = supportedExtensionsDictionary.Keys.ToArray();

        foreach (string extension in supportedExtensionsDictionaryKeys)
        {
            Dictionary<string, object> extensionDictionary = supportedExtensionsDictionary[extension];
            if ((string)extensionDictionary["Type"] == fileType)
            {
                filter += $"{extension[1..].ToUpper()} ";
            }
        }

        filter = filter.Remove(filter.LastIndexOf(" "));

        switch (fileType)
        {
            case "CONTAINER":
                FilterContainerExts = filter;
                break;
            case "VIDEO":
                FilterVideoExts = filter;
                break;
            case "AUDIO":
                FilterAudioExts = filter;
                break;
        }
    }

    public string DetermineInputFileType(string fileExt)
    {
        Dictionary<string, object> extensionDictionary = supportedExtensionsDictionary[fileExt];

        return (string)extensionDictionary["Type"];
    }

    public bool IsSupportedByMP4Box(string fileExt)
    {
        Dictionary<string, object> extensionDictionary = supportedExtensionsDictionary[fileExt];

        return (bool)extensionDictionary["MP4BoxSupport"];
    }
}
