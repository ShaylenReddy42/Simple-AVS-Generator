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

using System.Text;

namespace Simple_AVS_Generator.Core
{
    internal class AviSynthScript
    {
        private string? InputFile { get; set; }
        
        private bool Video { get; set; }
        private bool MuxOriginalVideo { get; set; }
        private bool NeedsToBeResized { get; set; }
        private bool Audio { get; set; }

        public string AVSScriptFile { get; set; }
        public string AVSScriptContent { get; private set; } = "";

        public AviSynthScript(Common common)
        {
            InputFile = common.FileName;
            Video = common.Video;
            MuxOriginalVideo = common.MuxOriginalVideo;
            NeedsToBeResized = common.NeedsToBeResized;
            Audio = common.Audio;

            AVSScriptFile = common.ScriptFile;
        }

        public void SetScriptContent()
        {
            StringBuilder sb = new();
            
            sb.Append($"i = \"{InputFile}\"\r\n\r\n");

            if (Video is true && MuxOriginalVideo is false)
            {
                sb.Append("v = LWLibavVideoSource(i).ConvertBits(8).ConvertToYV12()#.ShowFrameNumber()\r\n\r\n");
                sb.Append(ResizeVideo());
            }

            if (Audio)
            {
                sb.Append("a = LWLibavAudioSource(i).ConvertAudioToFloat()\r\n\r\n");
                sb.Append("a = Normalize(a, 1.0)\r\n\r\n");
                sb.Append("a = ConvertAudioTo16Bit(a)\r\n\r\n");
            }

            if ((Video is true && MuxOriginalVideo is false) && Audio)
            {
                sb.Append("o = AudioDub(v, a)\r\n\r\n");
                sb.Append("o = ConvertAudioTo16Bit(o)\r\n\r\n");
                sb.Append("o");
            }
            else if ((Video is true && MuxOriginalVideo is false) && Audio is false)
            {
                sb.Append("v");
            }
            else if (Audio is true && Video is false)
            {
                sb.Append("a");
            }

            AVSScriptContent = sb.ToString();
        }

        string ResizeVideo()
        {
            StringBuilder sb = new();

            sb.Append("# Calculate the target height based on a target width\r\n");
            sb.Append("aspectRatio  = float(Width(v)) / float(Height(v))\r\n");
            sb.Append($"targetWidth  = {(NeedsToBeResized ? "640" : "Width(v)")}\r\n");
            sb.Append("targetHeight = int(targetWidth / aspectRatio)\r\n");
            sb.Append("targetHeight = targetHeight + ((targetHeight % 2 != 0) ? 1 : 0)\r\n\r\n");
            
            sb.Append("v = Spline36Resize(v, targetWidth, targetHeight)\r\n\r\n");
            
            return sb.ToString();
        }
    }
}
