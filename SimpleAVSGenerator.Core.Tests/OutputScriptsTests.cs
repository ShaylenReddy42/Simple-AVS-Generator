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

using static SimpleAVSGenerator.Core.Support.Video;
using static SimpleAVSGenerator.Core.Support.Audio;

namespace SimpleAVSGenerator.Core.Tests;

public class OutputScriptsTests
{
    // VideoCodec | Expected video encoder
    public static IEnumerable<object[]> ConfigureVideoScript_ValidateWhichVideoEncoderIsUsed_TestData =
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
        Common common = new(@"C:\Users\User\Desktop\Sample.mp4")
        {
            OutputDir = @"C:\Users\User\Desktop\Temp\Sample\",
            Video = true,
            MuxOriginalVideo = false,
            VideoCodec = videoCodec,
            SourceFPS = 24,
            KeyframeIntervalInSeconds = 2
        };
        
        // Act
        OutputScripts output = new(common);
        output.ConfigureVideoScript();

        string? videoEncoderScriptContent = output.VideoEncoderScriptContent;
        
        // Assert
        Assert.Contains(expectedVideoEncoder, videoEncoderScriptContent);
    }

    // VideoCodec | SourceFPS | KeyframeIntervalInSeconds | Expected string in script content
    public static IEnumerable<object[]> ConfigureVideoScript_ValidateKeyframeIntervalInFrames_TestData =
    new[]
    {
        new object[] { "HEVC",     24, 2,  "-I 48"             },
        new object[] { "AV1",      25, 5,  "--kf-max-dist=125" },
        new object[] { "AVC",      30, 10, "-I 300"            },
        new object[] { "WhatsApp", 60, 10, "-I 600"            }
    };

    [Theory(DisplayName = "Validate Keyframe Interval In Frames")]
    [MemberData(nameof(ConfigureVideoScript_ValidateKeyframeIntervalInFrames_TestData))]
    public void ConfigureVideoScript_ValidateKeyframeIntervalInFrames
    (
        string videoCodec,
        int sourceFPS,
        int keyframeIntervalInSeconds,
        string expectedStringInScriptContent
    )
    {
        // Arrange
        Common common = new(@"C:\Users\User\Desktop\Sample.mp4")
        {
            OutputDir = @"C:\Users\User\Desktop\Temp\Sample\",
            Video = true,
            MuxOriginalVideo = false,
            VideoCodec = videoCodec,
            SourceFPS = sourceFPS,
            KeyframeIntervalInSeconds = keyframeIntervalInSeconds
        };

        // Act
        OutputScripts output = new(common);
        output.ConfigureVideoScript();

        string? videoEncoderScriptContent = output.VideoEncoderScriptContent;

        // Assert
        Assert.Contains(expectedStringInScriptContent, videoEncoderScriptContent);
    }

    // VideoCodec | Expected filename ending
    public static IEnumerable<object[]> ConfigureVideoScript_ValidateTheVideoScriptFilename_TestData =
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
        Common common = new(@"C:\Users\User\Desktop\Sample.mp4")
        {
            OutputDir = @"C:\Users\User\Desktop\Temp\Sample\",
            Video = true,
            MuxOriginalVideo = false,
            VideoCodec = videoCodec,
            SourceFPS = 24,
            KeyframeIntervalInSeconds = 2
        };

        // Act
        OutputScripts output = new(common);
        output.ConfigureVideoScript();

        string? videoEncoderScriptFile = output.VideoEncoderScriptFile;

        // Assert
        Assert.Equal($"{common.OutputDir}{expectedEndsWith}", videoEncoderScriptFile);
    }

    // AudioCodec | AudioExtension | Expected audio encoder
    public static IEnumerable<object[]> ConfigureAudioScript_ValidateWhichAudioEncoderIsUsed_TestData =
    new[]
    {
        new object[] { "AAC-LC", ".m4a", "qaac64"      },
        new object[] { "AAC-HE", ".m4a", "qaac64 --he" },
        new object[] { "OPUS",   ".ogg", "opusenc"     }
    };

    [Theory(DisplayName = "Validate Which Audio Encoder Is Used")]
    [MemberData(nameof(ConfigureAudioScript_ValidateWhichAudioEncoderIsUsed_TestData))]
    public void ConfigureAudioScript_ValidateWhichAudioEncoderIsUsed
    (
        string audioCodec,
        string audioExtension,
        string expectedAudioEncoder
    )
    {
        // Arrange
        Common common = new(@"C:\Users\User\Desktop\Sample.m4a")
        {
            OutputDir = @"C:\Users\User\Desktop\Temp\Sample\",
            Audio = true,
            AudioCodec = audioCodec,
            AudioBitrate = 128,
            AudioExtension = audioExtension
        };

        // Act
        OutputScripts output = new(common);
        output.ConfigureAudioScript();

        string? audioEncoderScriptContent = output.AudioEncoderScriptContent;

        // Assert
        Assert.Contains(expectedAudioEncoder, audioEncoderScriptContent);
    }

    // AudioCodec | AudioExtension | Expected filename ending
    public static IEnumerable<object[]> ConfigureAudioScript_ValidateTheAudioScriptFilename_TestData =
    new[]
    {
        new object[] { "AAC-LC", ".m4a", "Encode Audio [AAC-LC].cmd" },
        new object[] { "AAC-HE", ".m4a", "Encode Audio [AAC-HE].cmd" },
        new object[] { "OPUS",   ".ogg", "Encode Audio [OPUS].cmd"   }
    };

    [Theory(DisplayName = "Validate The Audio Script Filename")]
    [MemberData(nameof(ConfigureAudioScript_ValidateTheAudioScriptFilename_TestData))]
    public void ConfigureAudioScript_ValidateTheAudioScriptFilename
    (
        string audioCodec,
        string audioExtension,
        string expectedEndsWith
    )
    {
        // Arrange
        Common common = new(@"C:\Users\User\Desktop\Sample.m4a")
        {
            OutputDir = @"C:\Users\User\Desktop\Temp\Sample\",
            Audio = true,
            AudioCodec = audioCodec,
            AudioBitrate = 128,
            AudioExtension = audioExtension
        };

        // Act
        OutputScripts output = new(common);
        output.ConfigureAudioScript();

        string? audioEncoderScriptFile = output.AudioEncoderScriptFile;

        // Assert
        Assert.Equal($"{common.OutputDir}{expectedEndsWith}", audioEncoderScriptFile);
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
        Common common = new(@"C:\Users\User\Desktop\Sample.mp4")
        {
            OutputContainer = outputContainer
        };

        // Act
        OutputScripts output = new(common);
        output.ConfigureContainerScript();

        string? containerScriptContent = output.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedMultiplexer, containerScriptContent);
    }

    // Filename | Video | MuxOriginalVideo | VideoCodec | OutputContainer | Expected string in script
    public static IEnumerable<object[]> ConfigureContainerScript_ValidateVideoStringInScript_TestData =
    new[]
    {
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", false, "HEVC",         "MP4", $"-add \"%~dp0Video.265\":name="                       },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", false, "AV1",          "MP4", $"-add \"%~dp0Video.ivf\":name="                       },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", false, "AVC",          "MP4", $"-add \"%~dp0Video.264\":name="                       },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", true,  "Mux Original", "MP4", $"-add \"C:\\Users\\User\\Desktop\\Sample.mp4\"#video" },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", false, "HEVC",         "MKV", $"\"%~dp0Video.265\""                                  },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", false, "AV1",          "MKV", $"\"%~dp0Video.ivf\""                                  },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", false, "AVC",          "MKV", $"\"%~dp0Video.264\""                                  },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", true,  "Mux Original", "MKV", $"--no-audio \"C:\\Users\\User\\Desktop\\Sample.mp4\"" }
    };

    [Theory(DisplayName = "Validate Video String In Script")]
    [MemberData(nameof(ConfigureContainerScript_ValidateVideoStringInScript_TestData))]
    public void ConfigureContainerScript_ValidateVideoStringInScript
    (
        string fileName,
        bool muxOriginalVideo,
        string videoCodec,
        string outputContainer,
        string expectedVideoStringInScript
    )
    {
        // Arrange
        Common common = new(fileName)
        {
            OutputDir = @"C:\Users\User\Desktop\Temp\Sample\",
            Video = true,
            MuxOriginalVideo = muxOriginalVideo,
            VideoCodec = videoCodec,
            VideoExtension = outputVideoCodecsDictionary[videoCodec],
            OutputContainer = outputContainer
        };

        // Act
        OutputScripts output = new(common);
        output.ConfigureContainerScript();

        string? containerScriptContent = output.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedVideoStringInScript, containerScriptContent);
    }

    // Filename | AudioCodec | AudioLanguageKey | OutputContainer | Expected string in script
    public static IEnumerable<object[]> ConfigureContainerScript_ValidateAudioStringInScript_TestData =
    new[]
    {
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", "AAC-LC", "English",  "MP4", $"-add \"%~dp0Sample.m4a\":name=:lang=eng" },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", "AAC-HE", "Hindi",    "MP4", $"-add \"%~dp0Sample.m4a\":name=:lang=hin" },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", "OPUS",   "Japanese", "MP4", $"-add \"%~dp0Sample.ogg\":name=:lang=jpn" },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", "AAC-LC", "English",  "MKV", $"--language 0:eng \"%~dp0Sample.m4a\""    },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", "AAC-HE", "Hindi",    "MKV", $"--language 0:hin \"%~dp0Sample.m4a\""    },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", "OPUS",   "Japanese", "MKV", $"--language 0:jpn \"%~dp0Sample.ogg\""    }
    };

    [Theory(DisplayName = "Validate Audio String In Script")]
    [MemberData(nameof(ConfigureContainerScript_ValidateAudioStringInScript_TestData))]
    public void ConfigureContainerScript_ValidateAudioStringInScript
    (
        string fileName,
        string audioCodec,
        string audioLanguageKey,
        string outputContainer,
        string expectedAudioStringInScript
    )
    {
        // Arrange
        Common common = new(fileName)
        {
            OutputDir = @"C:\Users\User\Desktop\Temp\Sample\",
            Video = true,
            MuxOriginalVideo = false,
            VideoCodec = "HEVC",
            VideoExtension = outputVideoCodecsDictionary["HEVC"],
            Audio = true,
            AudioCodec = audioCodec,
            AudioExtension = outputAudioCodecsDictionary[audioCodec],
            AudioLanguage = languagesDictionary[audioLanguageKey],
            OutputContainer = outputContainer
        };

        // Act
        OutputScripts output = new(common);
        output.ConfigureContainerScript();

        string? containerScriptContent = output.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedAudioStringInScript, containerScriptContent);
    }

    // Filename | OutputContainer | Expected string in script
    public static IEnumerable<object[]> ConfigureContainerScript_ValidateOutputFileStringInScript_TestData =
    new[]
    {
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", "MP4", $"-new \"%~dp0Sample.mp4\"" },
        new object[] { @"C:\Users\User\Desktop\Sample.mp4", "MKV", $"-o \"%~dp0Sample.mkv\""   },
    };

    [Theory(DisplayName = "Validate Output File String In Script")]
    [MemberData(nameof(ConfigureContainerScript_ValidateOutputFileStringInScript_TestData))]
    public void ConfigureContainerScript_ValidateOutputFileStringInScript
    (
        string fileName,
        string outputContainer,
        string expectedOutputFileStringInScript
    )
    {
        // Arrange
        Common common = new(fileName)
        {
            OutputContainer = outputContainer
        };

        // Act
        OutputScripts output = new(common);
        output.ConfigureContainerScript();

        string? containerScriptContent = output.ContainerScriptContent;

        // Assert
        Assert.Contains(expectedOutputFileStringInScript, containerScriptContent);
    }

    // MuxOriginalVideo | OutputContainer | Expected filename ending
    public static IEnumerable<object[]> ConfigureContainerScript_ValidateTheContainerScriptFilename_TestData =
    new[]
    {
        new object[] { false, "MP4", $"MP4 Mux.cmd"                  },
        new object[] { true,  "MP4", $"MP4 Mux [Original Video].cmd" },
        new object[] { false, "MKV", $"MKV Mux.cmd"                  },
        new object[] { true,  "MKV", $"MKV Mux [Original Video].cmd" }
    };

    [Theory(DisplayName = "Validate The Container Script Filename")]
    [MemberData(nameof(ConfigureContainerScript_ValidateTheContainerScriptFilename_TestData))]
    public void ConfigureContainerScript_ValidateTheContainerScriptFilename
    (
        bool muxOriginalVideo,
        string outputContainer,
        string expectedEndsWith
    )
    {
        // Arrange
        Common common = new(@"C:\Users\User\Desktop\Sample.mp4")
        {
            OutputDir = @"C:\Users\User\Desktop\Temp\Sample\",
            MuxOriginalVideo = muxOriginalVideo,
            OutputContainer = outputContainer
        };

        // Act
        OutputScripts output = new(common);
        output.ConfigureContainerScript();

        string? containerScriptFile = output.ContainerScriptFile;

        // Assert
        Assert.Equal($"{common.OutputDir}{expectedEndsWith}", containerScriptFile);
    }
}
