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

using System.Collections.Generic;
using Xunit;

namespace SimpleAVSGeneratorCore.Tests;

public class InputFileTests
{
    // FileName | FileExt | FileNameOnly | FileType | IsSupportedByMP4Box
    public static IEnumerable<object[]> InputFileHandler_CheckIfPropertiesAreSetAccurately_TestData =>
    new[]
    {
        new object[] { @"C:\Users\User\Desktop\Sample1.mp4", ".mp4", "Sample1", "CONTAINER", true  },
        new object[] { @"C:\Users\User\Desktop\Sample2.mkv", ".mkv", "Sample2", "CONTAINER", false },
        new object[] { @"C:\Users\User\Desktop\Sample3.265", ".265", "Sample3", "VIDEO",     true  },
        new object[] { @"C:\Users\User\Desktop\Sample4.m4a", ".m4a", "Sample4", "AUDIO",     true  }
    };

    [Theory(DisplayName = "Check If Properties Are Set Accurately")]
    [MemberData(nameof(InputFileHandler_CheckIfPropertiesAreSetAccurately_TestData))]
    public void InputFileHandler_CheckIfPropertiesAreSetAccurately
    (
        string fileName,
        string fileExt,
        string fileNameOnly,
        string fileType,
        bool isSupportedByMP4Box
    )
    {
        // Arrange
        object[] expectedProperties = { fileName, fileExt, fileNameOnly, fileType, isSupportedByMP4Box };

        // Act
        InputFile input = new(fileName, @"C:\Users\User\Desktop\Temp\");
        string actualFileName     = input.FileInfo.FileName,
               actualFileExt      = input.FileInfo.FileExt,
               actualFileNameOnly = input.FileInfo.FileNameOnly,
               actualFileType     = input.FileInfo.FileType;
        bool actualIsSupportedByMP4Box = input.FileInfo.IsSupportedByMP4Box;

        object[] actualProperties = { actualFileName, actualFileExt, actualFileNameOnly, actualFileType, actualIsSupportedByMP4Box };

        // Assert
        Assert.Equal(expectedProperties, actualProperties);
    }

    [Theory(DisplayName = "Validate AVSMeter Script File And Content")]
    // FileName | Expected AVSMeter script file
    [InlineData(@"C:\Users\User\Desktop\Sample1.mp4", @"C:\Users\User\Desktop\Temp\Sample1\AVSMeter.cmd")]
    [InlineData(@"C:\Users\User\Desktop\Sample2.mp4", @"C:\Users\User\Desktop\Temp\Sample2\AVSMeter.cmd")]
    public void ValidateAVSMeterScriptFileAndContent
    (
        string fileName,
        string expectedScriptFile
    )
    {
        // Arrange
        string[] expectedScriptFileAndContent = { expectedScriptFile, $"AVSMeter64 \"%~dp0Script.avs\" -i -l" };

        InputFile input = new(fileName, @"C:\Users\User\Desktop\Temp\");

        // Act
        string actualScriptFile    = input.AVSMeterScriptFile,
               actualScriptContent = input.AVSMeterScriptContent;

        string[] actualScriptFileAndContent = { actualScriptFile, actualScriptContent };

        // Assert
        Assert.Equal(expectedScriptFileAndContent, actualScriptFileAndContent);
    }

    // Video | VideoCodec | Audio | OutputContainer | Expected scripts created
    public static IEnumerable<object?[]> CreateScripts_ValidateWhichScriptsWereCreated_TestData =
    new[]
    {
        new object?[] { true,  "HEVC",         true,  "MP4", "svac" },
        new object?[] { true,  "AV1",          false, "MP4", "svc"  },
        new object?[] { true,  "AVC",          false, null,  "sv"   },
        new object?[] { true,  "Mux Original", false, "MP4", "c"    },
        new object?[] { true,  "Mux Original", true,  "MP4", "sac"  },
        new object?[] { false, "",             true,  null,  "sa"   },
        new object?[] { false, "HEVC",         false, "MP4", ""     },
        new object?[] { false, "Mux Original", false, "MKV", ""     }

    };

    [Theory(DisplayName = "Validate Which Scripts Were Created")]
    [MemberData(nameof(CreateScripts_ValidateWhichScriptsWereCreated_TestData))]
    public void CreateScripts_ValidateWhichScriptsWereCreated
    (
        bool video,
        string videoCodec,
        bool audio,
        string? outputContainer,
        string expectedScriptsCreated
    )
    {
        // Arrange
        InputFile input = new(@"C:\Users\User\Desktop\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.Video.Enabled = video;
        input.Video.Codec = videoCodec;
        input.Video.KeyframeIntervalInSeconds = "2 Seconds";

        input.Audio.Enabled = audio;
        input.Audio.Codec = "AAC-LC";
        input.Audio.Bitrate = 128;
        input.Audio.Language = "English";

        input.OutputContainer = outputContainer;

        // Act
        input.CreateScripts(out string actualScriptsCreated);

        // Assert
        Assert.Equal(expectedScriptsCreated, actualScriptsCreated);
    }
}
