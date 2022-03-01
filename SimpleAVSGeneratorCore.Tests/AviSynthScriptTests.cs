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
        
        Common common = new(fileName, @"C:\Users\User\Desktop\Temp\")
        {
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

    // VideoCodec | Expected string in script
    public static IEnumerable<object[]> ResizeVideo_ValidateThatTargetWidthIsSetCorrectly_TestData =
    new[]
    {
        new object[] { "WhatsApp", "targetWidth  = 640"      },
        new object[] { "HEVC",     "targetWidth  = Width(v)" }
    };

    [Theory(DisplayName = "Validate That 'targetWidth' Is Set Correctly")]
    [MemberData(nameof(ResizeVideo_ValidateThatTargetWidthIsSetCorrectly_TestData))]
    public void ResizeVideo_ValidateThatTargetWidthIsSetCorrectly
    (
        string videoCodec,
        string expectedStringInScript
    )
    {
        // Arrange
        Common common = new(@"C:\Users\User\Desktop\Sample.mp4", @"C:\Users\User\Desktop\Temp\")
        {
            Video = true,
            VideoCodec = videoCodec
        };

        // Act
        AviSynthScript script = new(common);
        script.SetScriptContent();

        // Assert
        Assert.Contains(expectedStringInScript, script.AVSScriptContent);
    }
}
