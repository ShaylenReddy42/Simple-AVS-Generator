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

using System.Globalization;
using SimpleAVSGeneratorCore.Models;
using SimpleAVSGeneratorCore.Support;
using MediaInfo;

namespace SimpleAVSGeneratorCore;

public class InputFile
{
    public FileModel FileInfo;

    public string HomeDir { get; set; }
    public string OutputDir => $@"{HomeDir}{FileInfo.FileNameOnly}\";

    public string ScriptFile => $@"{OutputDir}Script.avs";

    //AVSMeter Properties
    public string AVSMeterScriptFile => $"{OutputDir}AVSMeter.cmd";
    public string AVSMeterScriptContent => $"AVSMeter64 \"%~dp0Script.avs\" -i -l";

    public VideoModel Video;

    public AudioModel Audio;

    public string? OutputContainer { get; set; }

    private MediaInfo.MediaInfo MI = new();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public InputFile(string fileName, string home)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        HomeDir = home;

        string fileExt = Path.GetExtension(fileName);

        Extensions exts = new();

        FileInfo = new()
        {
            FileName = fileName,
            FileType = exts.DetermineInputFileType(fileExt),
            IsSupportedByMP4Box = exts.IsSupportedByMP4Box(fileExt)
        };

#if RELEASE
        MI.Open(FileInfo.FileName);

        if (FileInfo.FileType is "CONTAINER")
        {
            Video = new()
            {
                SourceFPS = decimal.Parse(MI.Get(StreamKind.Video, 0, "FrameRate"), CultureInfo.InvariantCulture),
                SourceFrameCount = int.Parse(MI.Get(StreamKind.Video, 0, "FrameCount"))
            };

            Audio = new()
            {
                SourceChannels = GetSimpleAudioChannelLayout()
            };
        }
        else if (FileInfo.FileType is "VIDEO")
        {
            Video = new()
            {
                SourceFPS = decimal.Parse(MI.Get(StreamKind.Video, 0, "FrameRate"), CultureInfo.InvariantCulture),
                SourceFrameCount = int.Parse(MI.Get(StreamKind.Video, 0, "FrameCount"))
            };

            Audio = new()
            {
                SourceChannels = "2.0"
            };
        }
        else if (FileInfo.FileType is "AUDIO")
        {
            Video = new()
            {
                SourceFPS = 23.976M,
                SourceFrameCount = 0
            };

            Audio = new()
            {
                SourceChannels = GetSimpleAudioChannelLayout()
            };
        }
#elif DEBUG
        Video = new()
        {
            SourceFPS = 23.976M
        };
        
        Audio = new()
        {
            SourceChannels = "2.0"
        };
#endif
    }

    private string GetSimpleAudioChannelLayout()
    {
        // Ensures the value from MediaInfo e.g. 2/0/0 or 3/2/0.1 or 3/2/2.1 returns 2.0, 5.1 or 7.1 respectively
        // Those are channel positions. (Front/Rear/LFE) or (Front/Side/Rear+LFE)
        // Since this is a private method and is based on what's detected by MediaInfo,
        // I cannot add tests for this because it's not accessible

        string channelPositions = MI.Get(StreamKind.Audio, 0, "ChannelPositions/String2");
        channelPositions = channelPositions is "" ? MI.Get(StreamKind.Audio, 0, "Channels") : channelPositions;

        double channelLayout = 0.0;

        foreach (string ch in channelPositions.Split('/'))
        {
            channelLayout += double.Parse(ch, CultureInfo.InvariantCulture);
        }

        string sChannelLayout = $"{channelLayout:0.0}".Replace(',', '.');

        return sChannelLayout;
    }

#if RELEASE
    private void WriteFile(string outputFileName, string fileContents)
    {
        StreamWriter sw = new StreamWriter(outputFileName);
        sw.Write($"{(outputFileName.EndsWith(".cmd") ? "@ECHO off\r\n\r\n" : "")}{fileContents}");
        sw.Close();
    }
#endif

    public void CreateScripts(out string scriptsCreated)
    {
        // scriptsCreated is a variable that will be used for testing this function
        // Result could be in variable length, containing characters to indicated
        // which scripts were created e.g. svac
        // Key:
        // s indicates that the AviSynth script is created
        // v indicates that the Video Encoder script is created
        // a indicates that the Audio Encoder script is created
        // c indicates that the Container Muxing script is created
        
        scriptsCreated = "";

#if RELEASE
        Directory.CreateDirectory(OutputDir);
#endif

        AviSynthScript script = new(ScriptFile);
        
        script.SetScriptContent(FileInfo, Video, Audio);
        if (script.CreateAviSynthScript is true)
        {
            scriptsCreated += "s";
#if RELEASE
            WriteFile(script.AVSScriptFile, script.AVSScriptContent);

            WriteFile(AVSMeterScriptFile, AVSMeterScriptContent);
#endif
        }

        OutputScripts output = new();

        output.ConfigureVideoScript(FileInfo, Video, OutputDir);
        if (output.VideoEncoderScriptFile is not null && output.VideoEncoderScriptContent is not null)
        {
            scriptsCreated += "v";
#if RELEASE
            WriteFile(output.VideoEncoderScriptFile, output.VideoEncoderScriptContent);
#endif
        }

        output.ConfigureAudioScript(FileInfo, Audio, OutputDir);
        if (output.AudioEncoderScriptFile is not null && output.AudioEncoderScriptContent is not null)
        {
            scriptsCreated += "a";
#if RELEASE
            WriteFile(output.AudioEncoderScriptFile, output.AudioEncoderScriptContent);
#endif
        }

        output.ConfigureContainerScript(FileInfo, Video, Audio, OutputContainer, OutputDir);
        if (output.ContainerScriptFile is not null && output.ContainerScriptContent is not null)
        {
            scriptsCreated += "c";
#if RELEASE
            WriteFile(output.ContainerScriptFile, output.ContainerScriptContent);
#endif
        }

#if RELEASE
        if (scriptsCreated is "")
        {
            Directory.Delete(OutputDir);
        }
#endif
    }
}
