using System.Collections.Generic;
using Xunit;
using static SimpleAVSGenerator.Core.Enums;

namespace SimpleAVSGenerator.Core.Tests;

public class OutputScriptsTests
{
    // VideoCodec | Expected video encoder
    public static IEnumerable<object[]> ConfigureVideoScript_ValidateWhichEncoderIsUsed_TestData =
    new[]
    {
        new object[] { (int)VideoCodecs.HEVC,     "x265"   },
        new object[] { (int)VideoCodecs.AV1,      "aomenc" },
        new object[] { (int)VideoCodecs.AVC,      "x264"   },
        new object[] { (int)VideoCodecs.WhatsApp, "x264"   }
    };

    [Theory (DisplayName = "Validate Which Video Encoder is Used")]
    [MemberData(nameof(ConfigureVideoScript_ValidateWhichEncoderIsUsed_TestData))]
    public void ConfigureVideoScript_ValidateWhichEncoderIsUsed
    (
        int videoCodec,
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
        new object[] { (int)VideoCodecs.HEVC,     24, 2,  "-I 48"             },
        new object[] { (int)VideoCodecs.AV1,      25, 5,  "--kf-max-dist=125" },
        new object[] { (int)VideoCodecs.AVC,      30, 10, "-I 300"            },
        new object[] { (int)VideoCodecs.WhatsApp, 60, 10, "-I 600"            }
    };

    [Theory(DisplayName = "Validate Keyframe Interval In Frames")]
    [MemberData(nameof(ConfigureVideoScript_ValidateKeyframeIntervalInFrames_TestData))]
    public void ConfigureVideoScript_ValidateKeyframeIntervalInFrames
    (
        int videoCodec,
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
    public static IEnumerable<object[]> ConfigureVideoScript_ValidateTheScriptFilename_TestData =
    new[]
    {
        new object[] { (int)VideoCodecs.HEVC,     "Encode Video [HEVC].cmd"     },
        new object[] { (int)VideoCodecs.AV1,      "Encode Video [AV1].cmd"      },
        new object[] { (int)VideoCodecs.AVC,      "Encode Video [AVC].cmd"      },
        new object[] { (int)VideoCodecs.WhatsApp, "Encode Video [WhatsApp].cmd" }
    };

    [Theory(DisplayName = "Validate The End Of The Script Filename")]
    [MemberData(nameof(ConfigureVideoScript_ValidateTheScriptFilename_TestData))]
    public void ConfigureVideoScript_ValidateTheScriptFilename
    (
        int videoCodec,
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
}
