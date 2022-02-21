using System.Collections.Generic;
using Xunit;

namespace SimpleAVSGenerator.Core.Tests;

public class AviSynthScriptTests
{
    // Use Cases
    // FileName | Video | Mux Original Video | Audio | CreateAviSynthScript | EndsWith
    public static IEnumerable<object[]> AviSynthScript_CheckScriptContentForVariousUseCases_TestData =
    new[]
    {
        new object[] { @"C:\Users\User\Desktop\Sample1.mp4", true, false, true,  true,   "o", 21 },
        new object[] { @"C:\Users\User\Desktop\Sample2.mp4", true, false, false, true,   "v", 13 },
        new object[] { @"C:\Users\User\Desktop\Sample3.mp4", true, true,  true,  true,   "a", 9  },
        new object[] { @"C:\Users\User\Desktop\Sample4.mp4", true, true,  false, false, "\n", 3  },
        new object[] { @"C:\Users\User\Desktop\Sample5.264", true, false, false, true,   "v", 13 },
        new object[] { @"C:\Users\User\Desktop\Sample6.264", true, true,  false, false, "\n", 3  },
        new object[] { @"C:\Users\User\Desktop\Sample7.m4a", false, false, true, true,   "a", 9  }
    };

    [Theory (DisplayName = "Validate Script Content For Various Use Cases")]
    [MemberData(nameof(AviSynthScript_CheckScriptContentForVariousUseCases_TestData))]
    public void AviSynthScript_ValidateScriptContentForVariousUseCases
    (   
        string fileName,
        bool video,
        bool muxOriginalVideo,
        bool audio,
        bool expectedCreateAviSynthScript,
        string expectedEndsWith,
        int expectedLineCount
    )
    {
        // Arrange
        object[] expectedOutput = new object[] { expectedCreateAviSynthScript, expectedEndsWith, expectedLineCount };
        
        Common common = new(fileName)
        {
            OutputDir = @"C:\Users\User\Desktop\Temp\Sample\",
            Video = video,
            MuxOriginalVideo = muxOriginalVideo,
            Audio = audio
        };

        // Act
        AviSynthScript script = new(common);
        script.SetScriptContent();

        bool actualCreateAviSynthScript = script.CreateAviSynthScript;
        string actualEndsWith = script.AVSScriptContent.Substring(script.AVSScriptContent.Length - 1);
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

    [Theory (DisplayName = "Validate That 'targetWidth' Is Set Correctly")]
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
