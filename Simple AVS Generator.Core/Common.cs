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

using Simple_AVS_Generator.Core.Support;

namespace Simple_AVS_Generator.Core
{
    public class Common
    {
        public string FileName { get; private set; }
        public string FileExt { get; private set; }
        public string FileNameOnly { get; private set; }
        public int FileType { get; private set; }

        public string OutputDir { get; set; } = "";

        public bool IsSupportedByMP4Box { get; private set; }

        public string ScriptFile => $@"{OutputDir}Script.avs";

        //AVSMeter Properties
        public string AVSMeterScriptFile => $"{OutputDir}AVSMeter.cmd";
        public string AVSMeterScriptContent => $"AVSMeter64 \"%~dp0Script.avs\" -i -l";

        //Video Properties
        public bool Video { get; set; } = default;
        public int VideoCodec { get; set; }
        public int SourceFPS { get; set; }
        public int KeyframeIntervalInSeconds { get; set; }
        public bool NeedsToBeResized { get; set; } = default;
        public string VideoExtension { get; set; } = "";

        //Audio Properties
        public bool Audio { get; set; } = default;
        public int AudioCodec { get; set; }
        public int AudioBitrate { get; set; }
        public string AudioLanguage { get; set; } = "";
        public string AudioExtension { get; set; } = "";

        public int? OutputContainer { get; set; }
        public bool MuxOriginalVideo { get; set; } = default;

        public Common(string fileName)
        {
            FileName = fileName;
            FileExt = Path.GetExtension(FileName);
            FileNameOnly = Path.GetFileNameWithoutExtension(FileName);

            Extensions se = new();
            FileType = se.DetermineInputFileType(FileExt);
            IsSupportedByMP4Box = se.IsSupportedByMP4Box(FileExt);
        }
    }
}
