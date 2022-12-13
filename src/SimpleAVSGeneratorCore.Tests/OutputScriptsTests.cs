using System.Collections.Generic;
using Xunit;

namespace SimpleAVSGeneratorCore.Tests;

public class OutputScriptsTests
{
    // VideoCodec | Expected video encoder
    public static readonly IEnumerable<object[]> ConfigureVideoScript_ValidateWhichVideoEncoderIsUsed_TestData =
    new[]
    {
        new object[] { "HEVC",     "x265"   },
        new object[] { "AV1",      "aomenc" },
        new object[] { "AVC",      "x264"   },
        new object[] { "WhatsApp", "x264"   }
    };

    [Theory(DisplayName = "Validate Which Video Encoder Is Used")]
    [MemberData(nameof(ConfigureVideoScript_ValidateWhichVideoEncoderIsUsed_TestData))]
    public void ConfigureVideoScript_ValidateWhichVideoEncoderIsUsed
    (
        string videoCodec,
        string expectedVideoEncoder
    )
    {
        // Arrange
        InputFile input = new(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.Video.Enabled = true;
        input.Video.Codec = videoCodec;
        input.Video.KeyframeIntervalInSeconds = "2 Seconds";
        
        // Act
        OutputScripts output = new();
        output.ConfigureVideoScript(input.Video, input.OutputDir);

        string? videoEncoderScriptContent = output.VideoEncoderScriptContent;
        
        // Assert
        Assert.Contains(expectedVideoEncoder, videoEncoderScriptContent);
    }

    // VideoCodec | Expected filename ending
    public static readonly IEnumerable<object[]> ConfigureVideoScript_ValidateTheVideoScriptFilename_TestData =
    new[]
    {
        new object[] { "HEVC",     "Encode Video [HEVC].cmd"     },
        new object[] { "AV1",      "Encode Video [AV1].cmd"      },
        new object[] { "AVC",      "Encode Video [AVC].cmd"      },
        new object[] { "WhatsApp", "Encode Video [WhatsApp].cmd" }
    };

    [Theory(DisplayName = "Validate The Video Script Filename")]
    [MemberData(nameof(ConfigureVideoScript_ValidateTheVideoScriptFilename_TestData))]
    public void ConfigureVideoScript_ValidateTheVideoScriptFilename
    (
        string videoCodec,
        string expectedEndsWith
    )
    {
        // Arrange
        InputFile input = new(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.Video.Enabled = true;
        input.Video.Codec = videoCodec;

        // Act
        OutputScripts output = new();
        output.ConfigureVideoScript(input.Video, input.OutputDir);

        string? videoEncoderScriptFile = output.VideoEncoderScriptFile;

        // Assert
        Assert.Equal($"{input.OutputDir}{expectedEndsWith}", videoEncoderScriptFile);
    }

    // AudioCodec | Expected audio encoder
    public static readonly IEnumerable<object[]> ConfigureAudioScript_ValidateWhichAudioEncoderIsUsed_TestData =
    new[]
    {
        new object[] { "AAC-LC", "qaac64"      },
        new object[] { "AAC-HE", "qaac64 --he" },
        new object[] { "OPUS",   "opusenc"     }
    };

    [Theory(DisplayName = "Validate Which Audio Encoder Is Used")]
    [MemberData(nameof(ConfigureAudioScript_ValidateWhichAudioEncoderIsUsed_TestData))]
    public void ConfigureAudioScript_ValidateWhichAudioEncoderIsUsed
    (
        string audioCodec,
        string expectedAudioEncoder
    )
    {
        // Arrange
        InputFile input = new(@"Samples\Sample.m4a", @"C:\Users\User\Desktop\Temp\");

        input.Audio.Enabled = true;
        input.Audio.Codec = audioCodec;
        input.Audio.Bitrate = 128;

        // Act
        OutputScripts output = new();
        output.ConfigureAudioScript(input.FileInfo, input.Audio, input.OutputDir);

        string? audioEncoderScriptContent = output.AudioEncoderScriptContent;

        // Assert
        Assert.Contains(expectedAudioEncoder, audioEncoderScriptContent);
    }

    [Fact(DisplayName = "Check Audio Channel Mask for AAC-HE 7.1")]
    public void CheckAudioChannelMask()
    {
        // Arrange
        InputFile input = new(@"Samples\Sample DTS_X.mkv", @"C:\Users\User\Desktop\Temp\");

        input.Audio.Enabled = true;
        input.Audio.Codec = "AAC-HE";
        input.Audio.Bitrate = 256;

        // Act
        OutputScripts output = new();
        output.ConfigureAudioScript(input.FileInfo, input.Audio, input.OutputDir);

        string? audioEncoderScriptContent = output.AudioEncoderScriptContent;

        // Assert
        Assert.Contains("qaac64 --he --chanmask 0xff ", audioEncoderScriptContent);
    }

    // AudioCodec | Expected filename ending
    public static readonly IEnumerable<object[]> ConfigureAudioScript_ValidateTheAudioScriptFilename_TestData =
    new[]
    {
        new object[] { "AAC-LC", "Encode Audio [AAC-LC].cmd" },
        new object[] { "AAC-HE", "Encode Audio [AAC-HE].cmd" },
        new object[] { "OPUS",   "Encode Audio [OPUS].cmd"   }
    };

    [Theory(DisplayName = "Validate The Audio Script Filename")]
    [MemberData(nameof(ConfigureAudioScript_ValidateTheAudioScriptFilename_TestData))]
    public void ConfigureAudioScript_ValidateTheAudioScriptFilename
    (
        string audioCodec,
        string expectedEndsWith
    )
    {
        // Arrange
        InputFile input = new(@"Samples\Sample.m4a", @"C:\Users\User\Desktop\Temp\");

        input.Audio.Enabled = true;
        input.Audio.Codec = audioCodec;
        input.Audio.Bitrate = 128;

        // Act
        OutputScripts output = new();
        output.ConfigureAudioScript(input.FileInfo, input.Audio, input.OutputDir);

        string? audioEncoderScriptFile = output.AudioEncoderScriptFile;

        // Assert
        Assert.Equal($"{input.OutputDir}{expectedEndsWith}", audioEncoderScriptFile);
    }

    [Theory(DisplayName = "Validate Which Multiplexer Is Used")]
    // OutputContainer | Expected multiplexer
    [InlineData("MP4", "mp4box")]
    [InlineData("MKV", "mkvmerge")]
    public void ConfigureContainerScript_ValidateWhichMultiplexerIsUsed
    (
        string outputContainer,
        string expectedMultiplexer
    )
    {
        // Arrange
        InputFile input = new(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\")
        {
            OutputContainer = outputContainer
        };

        input.Video.Enabled = true;

        // Act
        OutputScripts output = new();
        output.ConfigureContainerScript(input.FileInfo, input.Video, input.Audio, input.OutputContainer, input.OutputDir);

        string? containerScriptContent = output.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedMultiplexer, containerScriptContent);
    }

    // VideoCodec | OutputContainer | Expected string in script
    public static readonly IEnumerable<object[]> ConfigureContainerScript_ValidateVideoStringInScript_TestData =
    new[]
    {
        new object[] { "HEVC",         "MP4", $"-add \"%~dp0Video.265\":name="      },
        new object[] { "AV1",          "MP4", $"-add \"%~dp0Video.ivf\":name="      },
        new object[] { "AVC",          "MP4", $"-add \"%~dp0Video.264\":name="      },
        new object[] { "Mux Original", "MP4", $"-add \"Samples\\Sample.mp4\"#video" },
        new object[] { "HEVC",         "MKV", $"\"%~dp0Video.265\""                 },
        new object[] { "AV1",          "MKV", $"\"%~dp0Video.ivf\""                 },
        new object[] { "AVC",          "MKV", $"\"%~dp0Video.264\""                 },
        new object[] { "Mux Original", "MKV", $"--no-audio \"Samples\\Sample.mp4\"" }
    };

    [Theory(DisplayName = "Validate Video String In Script")]
    [MemberData(nameof(ConfigureContainerScript_ValidateVideoStringInScript_TestData))]
    public void ConfigureContainerScript_ValidateVideoStringInScript
    (
        string videoCodec,
        string outputContainer,
        string expectedVideoStringInScript
    )
    {
        // Arrange
        InputFile input = new(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\")
        {
            OutputContainer = outputContainer
        };

        input.Video.Enabled = true;
        input.Video.Codec = videoCodec;

        // Act
        OutputScripts output = new();
        output.ConfigureContainerScript(input.FileInfo, input.Video, input.Audio, input.OutputContainer, input.OutputDir);

        string? containerScriptContent = output.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedVideoStringInScript, containerScriptContent);
    }

    // AudioCodec | AudioLanguageKey | OutputContainer | Expected string in script
    public static readonly IEnumerable<object[]> ConfigureContainerScript_ValidateAudioStringInScript_TestData =
    new[]
    {
        new object[] { "AAC-LC", "English",  "MP4", $"-add \"%~dp0Sample.m4a\":name=:lang=eng" },
        new object[] { "AAC-HE", "Hindi",    "MP4", $"-add \"%~dp0Sample.m4a\":name=:lang=hin" },
        new object[] { "OPUS",   "Japanese", "MP4", $"-add \"%~dp0Sample.ogg\":name=:lang=jpn" },
        new object[] { "AAC-LC", "English",  "MKV", $"--language 0:eng \"%~dp0Sample.m4a\""    },
        new object[] { "AAC-HE", "Hindi",    "MKV", $"--language 0:hin \"%~dp0Sample.m4a\""    },
        new object[] { "OPUS",   "Japanese", "MKV", $"--language 0:jpn \"%~dp0Sample.ogg\""    }
    };

    [Theory(DisplayName = "Validate Audio String In Script")]
    [MemberData(nameof(ConfigureContainerScript_ValidateAudioStringInScript_TestData))]
    public void ConfigureContainerScript_ValidateAudioStringInScript
    (
        string audioCodec,
        string audioLanguageKey,
        string outputContainer,
        string expectedAudioStringInScript
    )
    {
        // Arrange
        InputFile input = new(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\")
        {
            OutputContainer = outputContainer
        };

        input.Video.Enabled = true;
        input.Video.Codec = "HEVC";
        input.Audio.Enabled = true;
        input.Audio.Codec = audioCodec;
        input.Audio.Language = audioLanguageKey;

        // Act
        OutputScripts output = new();
        output.ConfigureContainerScript(input.FileInfo, input.Video, input.Audio, input.OutputContainer, input.OutputDir);

        string? containerScriptContent = output.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedAudioStringInScript, containerScriptContent);
    }

    // OutputContainer | Expected string in script
    public static readonly IEnumerable<object[]> ConfigureContainerScript_ValidateOutputFileStringInScript_TestData =
    new[]
    {
        new object[] { "MP4", $"-new \"%~dp0Sample.mp4\"" },
        new object[] { "MKV", $"-o \"%~dp0Sample.mkv\""   },
    };

    [Theory(DisplayName = "Validate Output File String In Script")]
    [MemberData(nameof(ConfigureContainerScript_ValidateOutputFileStringInScript_TestData))]
    public void ConfigureContainerScript_ValidateOutputFileStringInScript
    (
        string outputContainer,
        string expectedOutputFileStringInScript
    )
    {
        // Arrange
        InputFile input = new(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\")
        {
            OutputContainer = outputContainer
        };

        input.Video.Enabled = true;

        // Act
        OutputScripts output = new();
        output.ConfigureContainerScript(input.FileInfo, input.Video, input.Audio, input.OutputContainer, input.OutputDir);

        string? containerScriptContent = output.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedOutputFileStringInScript, containerScriptContent);
    }

    // VideoCodec | OutputContainer | Expected filename ending
    public static readonly IEnumerable<object[]> ConfigureContainerScript_ValidateTheContainerScriptFilename_TestData =
    new[]
    {
        new object[] { "HEVC",         "MP4", "MP4 Mux.cmd"                  },
        new object[] { "Mux Original", "MP4", "MP4 Mux [Original Video].cmd" },
        new object[] { "AV1",          "MKV", "MKV Mux.cmd"                  },
        new object[] { "Mux Original", "MKV", "MKV Mux [Original Video].cmd" }
    };

    [Theory(DisplayName = "Validate The Container Script Filename")]
    [MemberData(nameof(ConfigureContainerScript_ValidateTheContainerScriptFilename_TestData))]
    public void ConfigureContainerScript_ValidateTheContainerScriptFilename
    (
        string videoCodec,
        string outputContainer,
        string expectedEndsWith
    )
    {
        // Arrange
        InputFile input = new(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\")
        {
            OutputContainer = outputContainer
        };

        input.Video.Enabled = true;
        input.Video.Codec = videoCodec;

        // Act
        OutputScripts output = new();
        output.ConfigureContainerScript(input.FileInfo, input.Video, input.Audio, input.OutputContainer, input.OutputDir);

        string? containerScriptFile = output.ContainerScriptFile;

        // Assert
        Assert.Equal($"{input.OutputDir}{expectedEndsWith}", containerScriptFile);
    }
}
