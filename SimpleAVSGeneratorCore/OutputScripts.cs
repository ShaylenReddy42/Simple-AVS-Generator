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

using SimpleAVSGeneratorCore.Models;

namespace SimpleAVSGeneratorCore;

public class OutputScripts
{
    public string? VideoEncoderScriptFile { get; private set; }
    public string? VideoEncoderScriptContent { get; private set; }

    public string? AudioEncoderScriptFile { get; private set; }
    public string? AudioEncoderScriptContent { get; private set; }

    public string? ContainerScriptFile { get; private set; }
    public string? ContainerScriptContent { get; private set; }

    public OutputScripts() {}

    public void ConfigureVideoScript(FileModel fileInfo, VideoModel video, string outputDir)
    {
        if (video.Enabled is true && video.MuxOriginalVideo is false)
        {
            string vPipe    = "avs2pipemod -y4mp \"%~dp0Script.avs\" | ",
                   vEncoder = string.Empty;

            if (video.Codec is "HEVC")
            {
                vEncoder += $"x265 --profile main --preset slower --crf 26 -i 1 -I {video.KeyframeIntervalInFrames} --hist-scenecut --hist-threshold 0.02 ";
                vEncoder += $"--fades --aq-mode 4 --aq-motion --aud --no-open-gop --y4m -f 0 - \"%~dp0Video{video.Extension}\"";
            }
            else if (video.Codec is "AV1")
            {
                vEncoder += "aomenc --passes=1 --end-usage=q --cq-level=32 --target-bitrate=0 ";
                vEncoder += $"--enable-fwd-kf=1 --kf-max-dist={video.KeyframeIntervalInFrames} --verbose --ivf -o \"%~dp0Video{video.Extension}\" -";
            }
            else if (video.Codec is "AVC")
            {
                vEncoder += $"x264 --preset veryslow --crf 26 -i 1 -I {video.KeyframeIntervalInFrames} --bframes 3 --deblock -2:-1 --aq-mode 3 ";
                vEncoder += $"--aud --no-mbtree --demuxer y4m --frames 0 -o \"%~dp0Video{video.Extension}\" -";
            }
            else if (video.Codec is "WhatsApp")
            {
                vEncoder += $"x264 --profile baseline --preset veryslow --crf 26 -i 1 -I {video.KeyframeIntervalInFrames} --ref 1 --deblock -2:-1 ";
                vEncoder += $"--aud --no-mbtree --demuxer y4m --frames 0 -o \"%~dp0Video{video.Extension}\" -";
            }

            VideoEncoderScriptFile = $"{outputDir}Encode Video [{video.Codec}].cmd";
            VideoEncoderScriptContent = vPipe + vEncoder;
        }
    }

    public void ConfigureAudioScript(FileModel fileInfo, AudioModel audio, string outputDir)
    {
        if (audio.Enabled is true)
        {
            string aPipe    = "avs2pipemod -wav=16bit \"%~dp0Script.avs\" | ",
                   aEncoder = string.Empty;

            if (audio.Codec is "AAC-LC")
            {
                aEncoder += $"qaac64 --abr {audio.Bitrate} --ignorelength --no-delay ";
                aEncoder += $"-o \"%~dp0{fileInfo.FileNameOnly}{audio.Extension}\" - ";
            }
            else if (audio.Codec is "AAC-HE")
            {
                aEncoder += $"qaac64 --he --abr {audio.Bitrate} --ignorelength ";
                aEncoder += $"-o \"%~dp0{fileInfo.FileNameOnly}{audio.Extension}\" - ";
            }
            else if (audio.Codec is "OPUS")
            {
                aEncoder += $"opusenc --bitrate {audio.Bitrate} --ignorelength ";
                aEncoder += $"- \"%~dp0{fileInfo.FileNameOnly}{audio.Extension}\"";
            }

            AudioEncoderScriptFile = $"{outputDir}Encode Audio [{audio.Codec}].cmd";
            AudioEncoderScriptContent = aPipe + aEncoder;
        }
    }

    public void ConfigureContainerScript(FileModel fileInfo, VideoModel video, AudioModel audio, string? outputContainer, string outputDir)
    {
        string containerTemplate = outputContainer switch
        {
            "MP4" => $"mp4box $(video) $(audio) -new \"%~dp0{fileInfo.FileNameOnly}.mp4\"",
            "MKV" => $"mkvmerge -o \"%~dp0{fileInfo.FileNameOnly}.mkv\" $(video) $(audio)",
            _     => string.Empty
        };
        
        string videoTemplate = outputContainer switch
        {
            "MP4" => video.MuxOriginalVideo switch
            {
                true  => $"-add \"{fileInfo.FileName}\"#video",
                false => $"-add \"%~dp0Video{video.Extension}\":name="
            },
            "MKV" => video.MuxOriginalVideo switch
            {
                true  => $"--no-audio \"{fileInfo.FileName}\"",
                false => $"\"%~dp0Video{video.Extension}\""
            },
            _     => string.Empty
        };

        string audioTemplate = outputContainer switch
        {
            "MP4" => audio.Enabled switch
            {
                true  => $"-add \"%~dp0{fileInfo.FileNameOnly}{audio.Extension}\":name=:lang={audio.LanguageCode}",
                false => string.Empty
            },
            "MKV" => audio.Enabled switch
            {
                true  => $"--language 0:{audio.LanguageCode} \"%~dp0{fileInfo.FileNameOnly}{audio.Extension}\"",
                false => string.Empty
            },
            _     => string.Empty
        };

        if (containerTemplate is not "")
        {
            containerTemplate = containerTemplate.Replace("$(video)", videoTemplate);
            containerTemplate = containerTemplate.Replace("$(audio)", audioTemplate);
        }
        
        if (video.Enabled is true)
        {
            ContainerScriptFile = outputContainer is not null
                                ? $"{outputDir}{outputContainer} Mux{(video.MuxOriginalVideo ? " [Original Video]" : "")}.cmd"
                                : null;
            ContainerScriptContent = containerTemplate;
        }
    }
}
