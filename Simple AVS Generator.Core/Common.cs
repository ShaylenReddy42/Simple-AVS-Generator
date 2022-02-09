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
        public bool Video { get; set; }
        public int VideoCodec { get; set; }
        public int SourceFPS { get; set; }
        public int KeyframeIntervalInSeconds { get; set; }
        public bool NeedsToBeResized { get; set; }
        public string? VideoExtention { get; set; } = null;

        //Audio Properties
        public bool Audio { get; set; }
        public int AudioCodec { get; set; }
        public int AudioBitrate { get; set; }
        public string AudioLanguage { get; set; } = "";
        public string AudioExtension { get; set; } = "";

        public int? OutputContainer { get; set; }
        public bool MuxOriginalVideo { get; set; }

        public Common(string fileName)
        {
            FileName = fileName;
            FileExt = Path.GetExtension(FileName);
            FileNameOnly = Path.GetFileNameWithoutExtension(FileName);

            Extensions se = new();
            FileType = se.DetermineInputFileType(FileExt);

            SetIsSupportedByMP4Box();
        }

        public void SetIsSupportedByMP4Box()
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
}
