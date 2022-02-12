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
            bool createAviSynthScript,
            string endsWith
        )
        {
            // Arrange
            object[] expectedOutput = new object[] { createAviSynthScript, endsWith };
            
            // Act
            string noImpactOutputDir = @"C:\Users\User\Desktop\Temp\Sample\Script.avs";
            bool noImpactNeedsToBeResized = default;

            Common common = new(fileName);
            common.OutputDir = noImpactOutputDir;
            common.NeedsToBeResized = noImpactNeedsToBeResized;
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
    }
}
