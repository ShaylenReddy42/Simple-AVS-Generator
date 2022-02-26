using System.Collections.Generic;
using Xunit;
using static SimpleAVSGenerator.Core.Support.Video;
using static SimpleAVSGenerator.Core.Support.Audio;

namespace SimpleAVSGenerator.Core.Tests;

public class InputFileHandlerTests
{
#if DEBUG
    // Video | MuxOriginalVideo | Audio | OutputContainer | Expected scripts created
    public static IEnumerable<object?[]> CreateScripts_ValidateWhichScriptsWereCreated_TestData =
    new[]
    {
        new object?[] { true,  false, true,  "MP4", "svac" },
        new object?[] { true,  false, false, "MP4", "svc"  },
        new object?[] { true,  false, false, null,  "sv"   },
        new object?[] { true,  true,  false, "MP4", "c"    },
        new object?[] { true,  true,  true,  "MP4", "sac"  },
        new object?[] { false, false, true,  null,  "sa"   }
    };

    [Theory (DisplayName = "Validate Which Scripts Were Created")]
    [MemberData(nameof(CreateScripts_ValidateWhichScriptsWereCreated_TestData))]
    public void CreateScripts_ValidateWhichScriptsWereCreated
    (
        bool video,
        bool muxOriginalVideo,
        bool audio,
        string? outputContainer,
        string expectedScriptsCreated
    )
    {
        // Arrange
        InputFileHandler input = new(@"C:\Users\User\Desktop\Sample.mp4");

        input.common.OutputDir = @"C:\Users\User\Desktop\Temp\Sample\";

        input.common.Video = video;
        input.common.MuxOriginalVideo = muxOriginalVideo;
        input.common.VideoCodec = "AVC";
        input.common.SourceFPS = 24;
        input.common.KeyframeIntervalInSeconds = 2;
        input.common.VideoExtension = outputVideoCodecsDictionary["AVC"];

        input.common.Audio = audio;
        input.common.AudioCodec = "AAC-LC";
        input.common.AudioBitrate = 128;
        input.common.AudioLanguage = languagesDictionary["English"];
        input.common.AudioExtension = ".m4a";

        input.common.OutputContainer = outputContainer;

        // Act
        input.CreateScripts(out string actualScriptsCreated);

        // Assert
        Assert.Equal(expectedScriptsCreated, actualScriptsCreated);
    }
#endif
}
