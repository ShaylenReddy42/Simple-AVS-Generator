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

using SimpleAVSGeneratorCore.Support;
using static SimpleAVSGeneratorCore.Support.Video;
using static SimpleAVSGeneratorCore.Support.Audio;

namespace SimpleAVSGeneratorCore;

public class Common
{
    public string FileName { get; private set; }
    public string FileExt { get; private set; }
    public string FileNameOnly { get; private set; }
    public string FileType { get; private set; }

    public string OutputDir { get; set; } = string.Empty;

    public bool IsSupportedByMP4Box { get; private set; }

    public string ScriptFile => $@"{OutputDir}Script.avs";

    //AVSMeter Properties
    public string AVSMeterScriptFile => $"{OutputDir}AVSMeter.cmd";
    public string AVSMeterScriptContent => $"AVSMeter64 \"%~dp0Script.avs\" -i -l";

    //Video Properties
    public bool Video { get; set; } = default;
    public string VideoCodec { get; set; } = string.Empty;
    public int SourceFPS { get; set; }
    public int KeyframeIntervalInSeconds { get; set; }
    public bool NeedsToBeResized { get; set; } = default;
    public string VideoExtension => VideoCodec is not "" ? outputVideoCodecsDictionary[VideoCodec] : string.Empty; 

    //Audio Properties
    public bool Audio { get; set; } = default;
    public string AudioCodec { get; set; } = string.Empty;
    public int AudioBitrate { get; set; }
    public string AudioLanguage { get; set; } = string.Empty;
    public string AudioExtension => AudioCodec is not "" ? outputAudioCodecsDictionary[AudioCodec] : string.Empty;

    public string? OutputContainer { get; set; }
    public bool MuxOriginalVideo { get; set; } = default;

    public Common(string fileName)
    {
        FileName = fileName;
        FileExt = Path.GetExtension(FileName).ToLower();
        FileNameOnly = Path.GetFileNameWithoutExtension(FileName);

        Extensions se = new();
        FileType = se.DetermineInputFileType(FileExt);
        IsSupportedByMP4Box = se.IsSupportedByMP4Box(FileExt);
    }
}
