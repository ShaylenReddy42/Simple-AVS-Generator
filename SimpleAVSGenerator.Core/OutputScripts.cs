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

namespace SimpleAVSGenerator.Core;

public class OutputScripts
{
    private Common _common;
    
    public string? VideoEncoderScriptFile { get; private set; }
    public string? VideoEncoderScriptContent { get; private set; }

    public string? AudioEncoderScriptFile { get; private set; }
    public string? AudioEncoderScriptContent { get; private set; }

    public string? ContainerScriptFile { get; private set; }
    public string? ContainerScriptContent { get; private set; }

    public OutputScripts(Common common)
    {
        _common = common;
    }

    private int GetKeyframeIntervalInFrames() { return _common.SourceFPS * _common.KeyframeIntervalInSeconds; }

    public void ConfigureVideoScript()
    {
        if (_common.Video is true && _common.MuxOriginalVideo is false)
        {
            string? vPipe = "avs2pipemod -y4mp \"%~dp0Script.avs\" | ",
                    vEncoder = "",
                    vCmdFile = _common.OutputDir;

            if (_common.VideoCodec == (int)VideoCodecs.HEVC)
            {
                vEncoder += $"x265 --profile main --preset slower --crf 26 -i 1 -I {GetKeyframeIntervalInFrames()} --hist-scenecut --hist-threshold 0.02 ";
                vEncoder += "--fades --aq-mode 4 --aq-motion --aud --no-open-gop --y4m -f 0 - \"%~dp0Video.265\"";
                vCmdFile += "Encode Video [HEVC].cmd";
            }
            else if (_common.VideoCodec == (int)VideoCodecs.AV1)
            {
                vEncoder += "aomenc --passes=1 --end-usage=q --cq-level=32 --target-bitrate=0 ";
                vEncoder += $"--enable-fwd-kf=1 --kf-max-dist={GetKeyframeIntervalInFrames()} --verbose --ivf -o \"%~dp0Video.ivf\" -";
                vCmdFile += "Encode Video [AV1].cmd";
            }
            else if (_common.VideoCodec == (int)VideoCodecs.AVC)
            {
                vEncoder += $"x264 --preset veryslow --crf 26 -i 1 -I {GetKeyframeIntervalInFrames()} --bframes 3 --deblock -2:-1 --aq-mode 3 ";
                vEncoder += "--aud --no-mbtree --demuxer y4m --frames 0 -o \"%~dp0Video.264\" -";
                vCmdFile += "Encode Video [AVC].cmd";
            }
            else if (_common.VideoCodec == (int)VideoCodecs.WhatsApp)
            {
                vEncoder += $"x264 --profile baseline --preset veryslow --crf 26 -i 1 -I {GetKeyframeIntervalInFrames()} --ref 1 --deblock -2:-1 ";
                vEncoder += "--aud --no-mbtree --demuxer y4m --frames 0 -o \"%~dp0Video.264\" -";
                vCmdFile += "Encode Video [WhatsApp].cmd";
            }

            VideoEncoderScriptFile = vCmdFile;
            VideoEncoderScriptContent = vPipe + vEncoder;
        }
    }

    public void ConfigureAudioScript()
    {
        if (_common.Audio)
        {
            string? aPipe = "avs2pipemod -wav=16bit \"%~dp0Script.avs\" | ",
                    aEncoder = "",
                    aCmdFile = _common.OutputDir;

            if (_common.AudioCodec == (int)AudioCodecs.AAC_LC)
            {
                aEncoder += $"qaac64 --abr {_common.AudioBitrate} --ignorelength --no-delay ";
                aEncoder += $"-o \"%~dp0{_common.FileNameOnly}{_common.AudioExtension}\" - ";
                aCmdFile += "Encode Audio [AAC-LC].cmd";
            }
            else if (_common.AudioCodec == (int)AudioCodecs.AAC_HE)
            {
                aEncoder += $"qaac64 --he --abr {_common.AudioBitrate} --ignorelength ";
                aEncoder += $"-o \"%~dp0{_common.FileNameOnly}{_common.AudioExtension}\" - ";
                aCmdFile += "Encode Audio [AAC-HE].cmd";
            }
            else //OPUS
            {
                aEncoder += $"opusenc --bitrate {_common.AudioBitrate} --ignorelength ";
                aEncoder += $"- \"%~dp0{_common.FileNameOnly}{_common.AudioExtension}\"";
                aCmdFile += "Encode Audio [OPUS].cmd";
            }

            AudioEncoderScriptFile = aCmdFile;
            AudioEncoderScriptContent = aPipe + aEncoder;
        }
    }

    public void ConfigureContainerScript()
    {
        string? outputFileName = _common.OutputDir,
                fileContents = null;

        if (_common.OutputContainer == (int)OutputContainers.MP4)
        {
            string mp4V = _common.MuxOriginalVideo is false
                        ? $"-add \"%~dp0Video{_common.VideoExtension}\":name="
                        : $"-add \"{_common.FileName}\"#video",
                   mp4A = _common.Audio ? $"-add \"%~dp0{_common.FileNameOnly}{_common.AudioExtension}\":name=:lang={_common.AudioLanguage}" : "",
                   newmp4 = $"-new \"%~dp0{_common.FileNameOnly}.mp4\"";

            outputFileName += $"MP4 Mux{(_common.MuxOriginalVideo ? " [Original Video]" : "")}.cmd";
            fileContents = $"mp4box {mp4V} {mp4A} {newmp4}";
        }
        else if (_common.OutputContainer == (int)OutputContainers.MKV)
        {
            string mkvO = $"-o \"%~dp0{_common.FileNameOnly}.mkv\"",
                   mkvV = _common.MuxOriginalVideo is false
                        ? $"\"%~dp0Video{_common.VideoExtension}\""
                        : $"--no-audio \"{_common.FileName}\"",
                   mkvA = _common.Audio ? $"--language 0:{_common.AudioLanguage} \"%~dp0{_common.FileNameOnly}{_common.AudioExtension}\"" : "";

            outputFileName += $"MKV Mux{(_common.MuxOriginalVideo ? " [Original Video]" : "")}.cmd";
            fileContents = $"mkvmerge {mkvO} {mkvV} {mkvA}";
        }

        ContainerScriptFile = outputFileName;
        ContainerScriptContent = fileContents;
    }
}
