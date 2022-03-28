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
using SimpleAVSGeneratorCore.Support;

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

    public VideoModel Video = new();

    public AudioModel Audio = new();

    public string? OutputContainer { get; set; }

    public InputFile(string fileName, string home)
    {
        HomeDir = home;

        FileInfo = new()
        {
            FileName = fileName
        };

        Extensions se = new();
        FileInfo.FileType = se.DetermineInputFileType(FileInfo.FileExt);
        FileInfo.IsSupportedByMP4Box = se.IsSupportedByMP4Box(FileInfo.FileExt);
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
