using System.Collections.Generic;
using Xunit;

namespace Simple_AVS_Generator.Core.Tests
{
    public class AviSynthScriptTests
    {
        // Use Cases
        // FileName | Video | Mux Original Video | Audio | CreateAviSynthScript | EndsWith
        public static IEnumerable<object[]> AviSynthScript_CheckScriptContentForVariousUseCases_TestData =
        new[]
        {
            new object[] { @"C:\Users\User\Desktop\Sample1.mp4", true, false, true,  true,   "o" },
            new object[] { @"C:\Users\User\Desktop\Sample2.mp4", true, false, false, true,   "v" },
            new object[] { @"C:\Users\User\Desktop\Sample3.mp4", true, true,  true,  true,   "a" },
            new object[] { @"C:\Users\User\Desktop\Sample4.mp4", true, true,  false, false, "\n" },
            new object[] { @"C:\Users\User\Desktop\Sample5.264", true, false, false, true,   "v" },
            new object[] { @"C:\Users\User\Desktop\Sample6.264", true, true,  false, false, "\n" },
            new object[] { @"C:\Users\User\Desktop\Sample7.m4a", false, false, true, true,   "a" }
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
            string expectedEndsWith
        )
        {
            // Arrange
            object[] expectedOutput = new object[] { expectedCreateAviSynthScript, expectedEndsWith };
            
            // Act
            string noImpactOutputDir = @"C:\Users\User\Desktop\Temp\Sample\";

            Common common = new(fileName);
            common.OutputDir = noImpactOutputDir;
            common.Video = video;
            common.MuxOriginalVideo = muxOriginalVideo;
            common.Audio = audio;

            AviSynthScript script = new(common);
            script.SetScriptContent();

            bool actualCreateAviSynthScript = script.CreateAviSynthScript;
            string actualEndsWith = script.AVSScriptContent.Substring(script.AVSScriptContent.Length - 1);
            
            object[] actualOutput = new object[] { actualCreateAviSynthScript, actualEndsWith };

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

            // Act
            Common common = new(@"C:\Users\User\Desktop\Sample.mp4");
            common.OutputDir = @"C:\Users\User\Desktop\Temp\Sample\";
            common.Video = true;
            common.NeedsToBeResized = needsToBeResized;

            AviSynthScript script = new(common);
            script.SetScriptContent();

            string outputScript = script.AVSScriptContent;
            bool scriptDoesContainTheString = outputScript.Contains(scriptContainsThisString);

            // Assert
            Assert.True(scriptDoesContainTheString);
        }
    }
}
