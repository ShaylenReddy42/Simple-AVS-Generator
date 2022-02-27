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

    // ExtensionType | FileExtension | IsSupportedByMP4Box
    private readonly object[,] extensions =
    {
        { "CONTAINER", ".3gp",  true  },
        { "CONTAINER", ".3g2",  true  },
        { "CONTAINER", ".asf",  false },
        { "CONTAINER", ".avi",  true  },
        { "CONTAINER", ".flv",  false },
        { "CONTAINER", ".mp4",  true  },
        { "CONTAINER", ".m4v",  true  },
        { "CONTAINER", ".mkv",  false },
        { "CONTAINER", ".mov",  false },
        { "CONTAINER", ".m2t",  true  },
        { "CONTAINER", ".m2ts", true  },
        { "CONTAINER", ".mxf",  false },
        { "CONTAINER", ".ogm",  false },
        { "CONTAINER", ".rm",   false },
        { "CONTAINER", ".rmvb", false },
        { "CONTAINER", ".ts",   true  },
        { "CONTAINER", ".wmv",  false },

        { "VIDEO",     ".263",  true  },
        { "VIDEO",     ".h263", true  },
        { "VIDEO",     ".264",  true  },
        { "VIDEO",     ".h264", true  },
        { "VIDEO",     ".265",  true  },
        { "VIDEO",     ".h265", true  },
        { "VIDEO",     ".hevc", true  },
        { "VIDEO",     ".y4m",  false },

        { "AUDIO",     ".aa3",  false },
        { "AUDIO",     ".aac",  true  },
        { "AUDIO",     ".aif",  false },
        { "AUDIO",     ".ac3",  true  },
        { "AUDIO",     ".ape",  false },
        { "AUDIO",     ".dts",  false },
        { "AUDIO",     ".flac", true  },
        { "AUDIO",     ".m1a",  true  },
        { "AUDIO",     ".m2a",  true  },
        { "AUDIO",     ".mp2",  true  },
        { "AUDIO",     ".mp3",  true  },
        { "AUDIO",     ".m4a",  true  },
        { "AUDIO",     ".oma",  false },
        { "AUDIO",     ".opus", true  },
        { "AUDIO",     ".thd",  false },
        { "AUDIO",     ".tta",  false },
        { "AUDIO",     ".wav",  true  },
        { "AUDIO",     ".wma",  false }
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

        for (int i = 0; i < extensions.GetLength(0); i++)
        {
            if ((string)extensions[i, 0] == fileType)
            {
                support += $"*{extensions[i, 1]};";
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
        
        for (int i = 0; i < extensions.GetLength(0); i++)
        {
            if ((string)extensions[i, 0] == fileType)
            {
                filter += $"{extensions[i, 1].ToString()?[1..].ToUpper()} ";
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
        string fileType = string.Empty;
        
        for (int i = 0; i < extensions.GetLength(0); i++)
        {
            if ((string)extensions[i, 1] == fileExt)
            {
                fileType = (string)extensions[i, 0];
                break;
            }
        }

        return fileType;
    }

    public bool IsSupportedByMP4Box(string fileExt)
    {
        bool supported = default;

        for (int i = 0; i < extensions.GetLength(0); i++)
        {
            if ((string)extensions[i, 1] == fileExt)
            {
                supported = (bool)extensions[i, 2];
                break;
            }
        }

        return supported;
    }
}
