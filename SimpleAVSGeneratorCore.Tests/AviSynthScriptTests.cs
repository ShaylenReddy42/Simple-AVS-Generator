﻿/******************************************************************************
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

public class AviSynthScriptTests
{
    // Use Cases
    // FileName | Video | VideoCodec | Audio | CreateAviSynthScript | EndsWith
    public static IEnumerable<object[]> AviSynthScript_CheckScriptContentForVariousUseCases_TestData =
    new[]
    {
        new object[] { @"C:\Users\User\Desktop\Sample1.mp4", true,  "HEVC",         true,  true,  'o',  21 },
        new object[] { @"C:\Users\User\Desktop\Sample2.mp4", true,  "AV1",          false, true,  'v',  13 },
        new object[] { @"C:\Users\User\Desktop\Sample3.mp4", true,  "Mux Original", true,  true,  'a',   9 },
        new object[] { @"C:\Users\User\Desktop\Sample4.mp4", true,  "Mux Original", false, false, '\n',  3 },
        new object[] { @"C:\Users\User\Desktop\Sample5.264", true,  "AVC",          false, true,  'v',  13 },
        new object[] { @"C:\Users\User\Desktop\Sample6.264", true,  "Mux Original", false, false, '\n',  3 },
        new object[] { @"C:\Users\User\Desktop\Sample7.m4a", false, "",             true,  true,  'a',   9 }
    };

    [Theory(DisplayName = "Validate Script Content For Various Use Cases")]
    [MemberData(nameof(AviSynthScript_CheckScriptContentForVariousUseCases_TestData))]
    public void AviSynthScript_ValidateScriptContentForVariousUseCases
    (   
        string fileName,
        bool video,
        string videoCodec,
        bool audio,
        bool expectedCreateAviSynthScript,
        char expectedEndsWith,
        int expectedLineCount
    )
    {
        // Arrange
        object[] expectedOutput = new object[] { expectedCreateAviSynthScript, expectedEndsWith, expectedLineCount };
        
        Common common = new(fileName)
        {
            OutputDir = @"C:\Users\User\Desktop\Temp\Sample\",
            Video = video,
            VideoCodec = videoCodec,
            Audio = audio
        };

        // Act
        AviSynthScript script = new(common);
        script.SetScriptContent();

        bool actualCreateAviSynthScript = script.CreateAviSynthScript;
        char actualEndsWith = script.AVSScriptContent[^1];
        // Split the string using the line feed character which creates a string array
        // and get the length of the array
        int actualLineCount = (script.AVSScriptContent.Split('\n')).Length;
        
        object[] actualOutput = new object[] { actualCreateAviSynthScript, actualEndsWith, actualLineCount };

        // Assert
        Assert.Equal(expectedOutput, actualOutput);
    }

    // NeedsToBeResized | String the script should contain
    public static IEnumerable<object[]> ResizeVideo_ValidateThatTargetWidthIsSetCorrectly_TestData =
    new[]
    {
        new object[] { true,  "targetWidth  = 640"      },
        new object[] { false, "targetWidth  = Width(v)" }
    };

    [Theory(DisplayName = "Validate That 'targetWidth' Is Set Correctly")]
    [MemberData(nameof(ResizeVideo_ValidateThatTargetWidthIsSetCorrectly_TestData))]
    public void ResizeVideo_ValidateThatTargetWidthIsSetCorrectly
    (
        bool needsToBeResized,
        string scriptContainsThisString
    )
    {
        // Arrange
        // Since this test validates contents of a string, there's no expected values

        Common common = new(@"C:\Users\User\Desktop\Sample.mp4")
        {
            OutputDir = @"C:\Users\User\Desktop\Temp\Sample\",
            Video = true,
            NeedsToBeResized = needsToBeResized
        };

        // Act
        AviSynthScript script = new(common);
        script.SetScriptContent();

        string outputScript = script.AVSScriptContent;
        bool scriptDoesContainTheString = outputScript.Contains(scriptContainsThisString);

        // Assert
        Assert.True(scriptDoesContainTheString);
    }
}
