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

namespace SimpleAVSGeneratorCore;

public class AviSynthScript
{
    private Common _common;
    
    public bool CreateAviSynthScript { get; private set; } = default;
    public string AVSScriptFile { get; set; }
    public string AVSScriptContent { get; private set; } = "";

    public AviSynthScript(Common common)
    {
        _common = common;

        AVSScriptFile = _common.ScriptFile;
    }

    public void SetScriptContent()
    {
        StringBuilder sb = new();
        
        sb.Append($"i = \"{_common.FileName}\"\r\n\r\n");

        if (_common.Video is true && _common.MuxOriginalVideo is false)
        {
            sb.Append("v = LWLibavVideoSource(i).ConvertBits(8).ConvertToYV12()#.ShowFrameNumber()\r\n\r\n");
            sb.Append(ResizeVideo());
        }

        if (_common.Audio is true)
        {
            sb.Append("a = LWLibavAudioSource(i).ConvertAudioToFloat()\r\n\r\n");
            sb.Append("a = Normalize(a, 1.0)\r\n\r\n");
        }

        if ((_common.Video is true && _common.MuxOriginalVideo is false) && _common.Audio is true)
        {
            sb.Append("o = AudioDub(v, a)\r\n\r\n");
            sb.Append("o = ConvertAudioTo16Bit(o)\r\n\r\n");
            sb.Append("o");

            CreateAviSynthScript = true;
        }
        else if ((_common.Video is true && _common.MuxOriginalVideo is false) && _common.Audio is false)
        {
            sb.Append("v");

            CreateAviSynthScript = true;
        }
        else if (_common.Audio is true && (_common.Video is false || (_common.Video is true && _common.MuxOriginalVideo is true)))
        {
            sb.Append("a = ConvertAudioTo16Bit(a)\r\n\r\n");
            sb.Append("a");

            CreateAviSynthScript = true;
        }

        AVSScriptContent = sb.ToString();
    }

    string ResizeVideo()
    {
        StringBuilder sb = new();

        sb.Append("# Calculate the target height based on a target width\r\n");
        sb.Append("aspectRatio  = float(Width(v)) / float(Height(v))\r\n");
        sb.Append($"targetWidth  = {(_common.NeedsToBeResized ? "640" : "Width(v)")}\r\n");
        sb.Append("targetHeight = int(targetWidth / aspectRatio)\r\n");
        sb.Append("targetHeight = targetHeight + ((targetHeight % 2 != 0) ? 1 : 0)\r\n\r\n");
        
        sb.Append("v = Spline36Resize(v, targetWidth, targetHeight)\r\n\r\n");
        
        return sb.ToString();
    }
}
