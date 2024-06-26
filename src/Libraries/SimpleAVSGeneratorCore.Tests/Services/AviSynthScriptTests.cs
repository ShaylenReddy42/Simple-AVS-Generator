﻿using SimpleAVSGeneratorCore.Services;
using SimpleAVSGeneratorCore.Tests.Fixtures;

namespace SimpleAVSGeneratorCore.Tests.Services;

[Collection("CommonDependencyInjectionCollection")]
public class AviSynthScriptTests(CommonDependencyInjectionFixture commonDependencyInjectionFixture)
{

    // Use Cases
    // FileName | Video | VideoCodec | Audio | CreateAviSynthScript | EndsWith | LineCount
    public static TheoryData<string, bool, string, bool, bool, char, int> AviSynthScript_CheckScriptContentForVariousUseCases_TestData =>
        new()
        {
            { @"Samples\Sample.mp4", true,  "HEVC",         true,  true,  'o',  25 },
            { @"Samples\Sample.mp4", true,  "AV1",          false, true,  'v',  17 },
            { @"Samples\Sample.mp4", true,  "Mux Original", true,  true,  'a',   9 },
            { @"Samples\Sample.mp4", true,  "Mux Original", false, false, '\n',  3 },
            { @"Samples\Sample.265", true,  "AVC",          false, true,  'v',  17 },
            { @"Samples\Sample.265", true,  "Mux Original", false, false, '\n',  3 },
            { @"Samples\Sample.m4a", false, "",             true,  true,  'a',   9 }
        };

    [Theory(DisplayName = "Validate Script Content For Various Use Cases")]
    [MemberData(nameof(AviSynthScript_CheckScriptContentForVariousUseCases_TestData))]
    public async Task AviSynthScript_ValidateScriptContentForVariousUseCases(
        string fileName,
        bool video,
        string videoCodec,
        bool audio,
        bool expectedCreateAviSynthScript,
        char expectedEndsWith,
        int expectedLineCount)
    {
        // Arrange
        var expectedOutput = new object[] { expectedCreateAviSynthScript, expectedEndsWith, expectedLineCount };

        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(fileName, @"C:\Users\User\Desktop\Temp\");

        input.Video.Enabled = video;
        input.Video.Codec = videoCodec;
        input.Audio.Enabled = audio;

        // Act
        var aviSynthScriptService = new AviSynthScriptService();
        await aviSynthScriptService.SetScriptContentAsync(input.FileInfo, input.Video, input.Audio);

        bool actualCreateAviSynthScript = aviSynthScriptService.CreateAviSynthScript;
        char actualEndsWith = aviSynthScriptService.AVSScriptContent[^1];
        // Split the string using the line feed character which creates a string array
        // and get the length of the array
        int actualLineCount = aviSynthScriptService.AVSScriptContent.Split('\n').Length;

        var actualOutput = new object[] { actualCreateAviSynthScript, actualEndsWith, actualLineCount };

        // Assert
        Assert.Equal(expectedOutput, actualOutput);
    }

    // VideoCodec | Expected string in script
    public static TheoryData<string, string> ResizeVideo_ValidateThatTargetWidthIsSetCorrectly_TestData =>
        new()
        {
            { "WhatsApp", "targetWidth  = 640"      },
            { "HEVC",     "targetWidth  = Width(v)" }
        };

    [Theory(DisplayName = "Validate That 'targetWidth' Is Set Correctly")]
    [MemberData(nameof(ResizeVideo_ValidateThatTargetWidthIsSetCorrectly_TestData))]
    public async Task ResizeVideo_ValidateThatTargetWidthIsSetCorrectly(
        string videoCodec,
        string expectedStringInScript)
    {
        // Arrange
        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.Video.Enabled = true;
        input.Video.Codec = videoCodec;

        // Act
        var aviSynthScriptService = new AviSynthScriptService();
        await aviSynthScriptService.SetScriptContentAsync(input.FileInfo, input.Video, input.Audio);

        // Assert
        Assert.Contains(expectedStringInScript, aviSynthScriptService.AVSScriptContent);
    }
}
