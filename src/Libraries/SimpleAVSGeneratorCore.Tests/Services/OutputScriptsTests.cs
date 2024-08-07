﻿using SimpleAVSGeneratorCore.Constants;
using SimpleAVSGeneratorCore.Services;
using SimpleAVSGeneratorCore.Tests.Fixtures;

namespace SimpleAVSGeneratorCore.Tests.Services;

[Collection("CommonDependencyInjectionCollection")]
public class OutputScriptsTests(CommonDependencyInjectionFixture commonDependencyInjectionFixture)
{

    // VideoCodec | Expected video encoder
    public static TheoryData<string, string> ConfigureVideoScript_ValidateWhichVideoEncoderIsUsed_TestData =>
    new()
    {
        { "HEVC",     "x265"   },
        { "AV1",      "aomenc" },
        { "AVC",      "x264"   },
        { "WhatsApp", "x264"   }
    };

    [Theory(DisplayName = "Validate Which Video Encoder Is Used")]
    [MemberData(nameof(ConfigureVideoScript_ValidateWhichVideoEncoderIsUsed_TestData))]
    public async Task ConfigureVideoScript_ValidateWhichVideoEncoderIsUsed(
        string videoCodec,
        string expectedVideoEncoder)
    {
        // Arrange
        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.Video.Enabled = true;
        input.Video.Codec = videoCodec;
        input.Video.KeyframeIntervalInSeconds = "2 Seconds";

        // Act
        var outputScriptsService = new OutputScriptsService();
        await outputScriptsService.ConfigureVideoScriptAsync(input.Video, input.OutputDir);

        string? videoEncoderScriptContent = outputScriptsService.VideoEncoderScriptContent;

        // Assert
        Assert.Contains(expectedVideoEncoder, videoEncoderScriptContent);
    }

    // VideoCodec | Expected filename ending
    public static TheoryData<string, string> ConfigureVideoScript_ValidateTheVideoScriptFilename_TestData =>
        new()
        {
            { "HEVC",     "Encode Video [HEVC].cmd"     },
            { "AV1",      "Encode Video [AV1].cmd"      },
            { "AVC",      "Encode Video [AVC].cmd"      },
            { "WhatsApp", "Encode Video [WhatsApp].cmd" }
        };

    [Theory(DisplayName = "Validate The Video Script Filename")]
    [MemberData(nameof(ConfigureVideoScript_ValidateTheVideoScriptFilename_TestData))]
    public async Task ConfigureVideoScript_ValidateTheVideoScriptFilename(
        string videoCodec,
        string expectedEndsWith)
    {
        // Arrange
        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.Video.Enabled = true;
        input.Video.Codec = videoCodec;

        // Act
        var outputScriptsService = new OutputScriptsService();
        await outputScriptsService.ConfigureVideoScriptAsync(input.Video, input.OutputDir);

        string? videoEncoderScriptFile = outputScriptsService.VideoEncoderScriptFile;

        // Assert
        Assert.Equal(Path.Combine(input.OutputDir, expectedEndsWith), videoEncoderScriptFile);
    }

    // AudioCodec | Expected audio encoder
    public static TheoryData<string, string> ConfigureAudioScript_ValidateWhichAudioEncoderIsUsed_TestData =>
        new()
        {
            { SupportedOutputAudioCodecs.AacLc, "qaac64"      },
            { SupportedOutputAudioCodecs.AacHe, "qaac64 --he" },
            { SupportedOutputAudioCodecs.Opus,  "opusenc"     }
        };

    [Theory(DisplayName = "Validate Which Audio Encoder Is Used")]
    [MemberData(nameof(ConfigureAudioScript_ValidateWhichAudioEncoderIsUsed_TestData))]
    public async Task ConfigureAudioScript_ValidateWhichAudioEncoderIsUsed(
        string audioCodec,
        string expectedAudioEncoder)
    {
        // Arrange
        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(@"Samples\Sample.m4a", @"C:\Users\User\Desktop\Temp\");

        input.Audio.Enabled = true;
        input.Audio.Codec = audioCodec;
        input.Audio.Bitrate = 128;

        // Act
        var outputScriptsService = new OutputScriptsService();
        await outputScriptsService.ConfigureAudioScriptAsync(input.FileInfo, input.Audio, input.OutputDir);

        string? audioEncoderScriptContent = outputScriptsService.AudioEncoderScriptContent;

        // Assert
        Assert.Contains(expectedAudioEncoder, audioEncoderScriptContent);
    }

    [Fact(DisplayName = "Check Audio Channel Mask for AAC-HE 7.1")]
    public async Task CheckAudioChannelMask()
    {
        // Arrange
        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(@"Samples\Sample DTS_X.mkv", @"C:\Users\User\Desktop\Temp\");

        input.Audio.Enabled = true;
        input.Audio.Codec = "AAC-HE";
        input.Audio.Bitrate = 256;

        // Act
        var outputScriptsService = new OutputScriptsService();
        await outputScriptsService.ConfigureAudioScriptAsync(input.FileInfo, input.Audio, input.OutputDir);

        string? audioEncoderScriptContent = outputScriptsService.AudioEncoderScriptContent;

        // Assert
        Assert.Contains("qaac64 --he --chanmask 0xff ", audioEncoderScriptContent);
    }

    // AudioCodec | Expected filename ending
    public static TheoryData<string, string> ConfigureAudioScript_ValidateTheAudioScriptFilename_TestData =>
        new()
        {
            { SupportedOutputAudioCodecs.AacLc, "Encode Audio [AAC-LC].cmd" },
            { SupportedOutputAudioCodecs.AacHe, "Encode Audio [AAC-HE].cmd" },
            { SupportedOutputAudioCodecs.Opus,  "Encode Audio [OPUS].cmd"   }
        };

    [Theory(DisplayName = "Validate The Audio Script Filename")]
    [MemberData(nameof(ConfigureAudioScript_ValidateTheAudioScriptFilename_TestData))]
    public async Task ConfigureAudioScript_ValidateTheAudioScriptFilename(
        string audioCodec,
        string expectedEndsWith)
    {
        // Arrange
        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(@"Samples\Sample.m4a", @"C:\Users\User\Desktop\Temp\");

        input.Audio.Enabled = true;
        input.Audio.Codec = audioCodec;
        input.Audio.Bitrate = 128;

        // Act
        var outputScriptsService = new OutputScriptsService();
        await outputScriptsService.ConfigureAudioScriptAsync(input.FileInfo, input.Audio, input.OutputDir);

        string? audioEncoderScriptFile = outputScriptsService.AudioEncoderScriptFile;

        // Assert
        Assert.Equal(Path.Combine(input.OutputDir, expectedEndsWith), audioEncoderScriptFile);
    }

    [Theory(DisplayName = "Validate Which Multiplexer Is Used")]
    // OutputContainer | Expected multiplexer
    [InlineData("MP4", "mp4box")]
    [InlineData("MKV", "mkvmerge")]
    public async Task ConfigureContainerScript_ValidateWhichMultiplexerIsUsed(
        string outputContainer,
        string expectedMultiplexer)
    {
        // Arrange
        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.OutputContainer = outputContainer;
        input.Video.Enabled = true;

        // Act
        var outputScriptsService = new OutputScriptsService();
        await outputScriptsService.ConfigureContainerScriptAsync(input.FileInfo, input.Video, input.Audio, input.OutputContainer, input.OutputDir);

        string? containerScriptContent = outputScriptsService.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedMultiplexer, containerScriptContent);
    }

    // VideoCodec | OutputContainer | Expected string in script
    public static TheoryData<string, string, string> ConfigureContainerScript_ValidateVideoStringInScript_TestData =>
        new()
        {
            { "HEVC",         "MP4", $"-add \"%~dp0Video.265\":name="      },
            { "AV1",          "MP4", $"-add \"%~dp0Video.ivf\":name="      },
            { "AVC",          "MP4", $"-add \"%~dp0Video.264\":name="      },
            { "Mux Original", "MP4", $"-add \"Samples\\Sample.mp4\"#video" },
            { "HEVC",         "MKV", $"\"%~dp0Video.265\""                 },
            { "AV1",          "MKV", $"\"%~dp0Video.ivf\""                 },
            { "AVC",          "MKV", $"\"%~dp0Video.264\""                 },
            { "Mux Original", "MKV", $"--no-audio \"Samples\\Sample.mp4\"" }
        };

    [Theory(DisplayName = "Validate Video String In Script")]
    [MemberData(nameof(ConfigureContainerScript_ValidateVideoStringInScript_TestData))]
    public async Task ConfigureContainerScript_ValidateVideoStringInScript(
        string videoCodec,
        string outputContainer,
        string expectedVideoStringInScript)
    {
        // Arrange
        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.OutputContainer = outputContainer;
        input.Video.Enabled = true;
        input.Video.Codec = videoCodec;

        // Act
        var outputScriptsService = new OutputScriptsService();
        await outputScriptsService.ConfigureContainerScriptAsync(input.FileInfo, input.Video, input.Audio, input.OutputContainer, input.OutputDir);

        string? containerScriptContent = outputScriptsService.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedVideoStringInScript, containerScriptContent);
    }

    // AudioCodec | AudioLanguageKey | OutputContainer | Expected string in script
    public static TheoryData<string, string, string, string> ConfigureContainerScript_ValidateAudioStringInScript_TestData =>
        new()
        {
            { SupportedOutputAudioCodecs.AacLc, "English",  "MP4", $"-add \"%~dp0Sample.m4a\":name=:lang=eng" },
            { SupportedOutputAudioCodecs.AacHe, "Hindi",    "MP4", $"-add \"%~dp0Sample.m4a\":name=:lang=hin" },
            { SupportedOutputAudioCodecs.Opus,  "Japanese", "MP4", $"-add \"%~dp0Sample.ogg\":name=:lang=jpn" },
            { SupportedOutputAudioCodecs.AacLc, "English",  "MKV", $"--language 0:eng \"%~dp0Sample.m4a\""    },
            { SupportedOutputAudioCodecs.AacHe, "Hindi",    "MKV", $"--language 0:hin \"%~dp0Sample.m4a\""    },
            { SupportedOutputAudioCodecs.Opus,  "Japanese", "MKV", $"--language 0:jpn \"%~dp0Sample.ogg\""    }
        };

    [Theory(DisplayName = "Validate Audio String In Script")]
    [MemberData(nameof(ConfigureContainerScript_ValidateAudioStringInScript_TestData))]
    public async Task ConfigureContainerScript_ValidateAudioStringInScript(
        string audioCodec,
        string audioLanguageKey,
        string outputContainer,
        string expectedAudioStringInScript)
    {
        // Arrange
        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.OutputContainer = outputContainer;
        input.Video.Enabled = true;
        input.Video.Codec = "HEVC";
        input.Audio.Enabled = true;
        input.Audio.Codec = audioCodec;
        input.Audio.Language = audioLanguageKey;

        // Act
        var outputScriptsService = new OutputScriptsService();
        await outputScriptsService.ConfigureContainerScriptAsync(input.FileInfo, input.Video, input.Audio, input.OutputContainer, input.OutputDir);

        string? containerScriptContent = outputScriptsService.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedAudioStringInScript, containerScriptContent);
    }

    // OutputContainer | Expected string in script
    public static TheoryData<string, string> ConfigureContainerScript_ValidateOutputFileStringInScript_TestData =>
        new()
        {
            { "MP4", $"-new \"%~dp0Sample.mp4\"" },
            { "MKV", $"-o \"%~dp0Sample.mkv\""   },
        };

    [Theory(DisplayName = "Validate Output File String In Script")]
    [MemberData(nameof(ConfigureContainerScript_ValidateOutputFileStringInScript_TestData))]
    public async Task ConfigureContainerScript_ValidateOutputFileStringInScript(
        string outputContainer,
        string expectedOutputFileStringInScript)
    {
        // Arrange
        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.OutputContainer = outputContainer;
        input.Video.Enabled = true;

        // Act
        var outputScriptsService = new OutputScriptsService();
        await outputScriptsService.ConfigureContainerScriptAsync(input.FileInfo, input.Video, input.Audio, input.OutputContainer, input.OutputDir);

        string? containerScriptContent = outputScriptsService.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedOutputFileStringInScript, containerScriptContent);
    }

    // VideoCodec | OutputContainer | Expected filename ending
    public static TheoryData<string, string, string> ConfigureContainerScript_ValidateTheContainerScriptFilename_TestData =>
        new()
        {
            { "HEVC",         "MP4", "MP4 Mux.cmd"                  },
            { "Mux Original", "MP4", "MP4 Mux [Original Video].cmd" },
            { "AV1",          "MKV", "MKV Mux.cmd"                  },
            { "Mux Original", "MKV", "MKV Mux [Original Video].cmd" }
        };

    [Theory(DisplayName = "Validate The Container Script Filename")]
    [MemberData(nameof(ConfigureContainerScript_ValidateTheContainerScriptFilename_TestData))]
    public async Task ConfigureContainerScript_ValidateTheContainerScriptFilename(
        string videoCodec,
        string outputContainer,
        string expectedEndsWith)
    {
        // Arrange
        var input = await commonDependencyInjectionFixture.InputFileHandlerServiceInstance.CreateInputFileAsync(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.OutputContainer = outputContainer;
        input.Video.Enabled = true;
        input.Video.Codec = videoCodec;

        // Act
        var outputScriptsService = new OutputScriptsService();
        await outputScriptsService.ConfigureContainerScriptAsync(input.FileInfo, input.Video, input.Audio, input.OutputContainer, input.OutputDir);

        string? containerScriptFile = outputScriptsService.ContainerScriptFile;

        // Assert
        Assert.Equal(Path.Combine(input.OutputDir, expectedEndsWith), containerScriptFile);
    }
}
