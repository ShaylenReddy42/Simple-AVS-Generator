using System.Collections.Generic;
using Xunit;
using static SimpleAVSGenerator.Core.Enums;
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
        new object?[] { true,  false, true, (int)OutputContainers.MP4, "svac" },
        new object?[] { true,  true,  true, (int)OutputContainers.MP4, "sac"  },
        new object?[] { false, false, true, null                     , "sa"   },
    };

    [Theory]
    [MemberData(nameof(CreateScripts_ValidateWhichScriptsWereCreated_TestData))]
    public void CreateScripts_ValidateWhichScriptsWereCreated
    (
        bool video,
        bool muxOriginalVideo,
        bool audio,
        int? outputContainer,
        string expectedScriptsCreated
    )
    {
        // Arrange
        InputFileHandler input = new(@"C:\Users\User\Desktop\Sample.mp4");

        input.common.OutputDir = @"C:\Users\User\Desktop\Temp\Sample\";

        input.common.Video = video;
        input.common.MuxOriginalVideo = muxOriginalVideo;
        input.common.VideoCodec = (int)VideoCodecs.AVC;
        input.common.SourceFPS = 24;
        input.common.KeyframeIntervalInSeconds = 2;
        input.common.VideoExtension = outputVideoCodecs[(int)VideoCodecs.AVC, 0];

        input.common.Audio = audio;
        input.common.AudioCodec = (int)AudioCodecs.AAC_LC;
        input.common.AudioBitrate = 128;
        input.common.AudioLanguage = languages[0, 0];
        input.common.AudioExtension = outputAudioCodecs[(int)AudioCodecs.AAC_LC, 0];

        input.common.OutputContainer = outputContainer;

        // Act
        input.CreateScripts(out string actualScriptsCreated);

        // Assert
        Assert.Equal(expectedScriptsCreated, actualScriptsCreated);
    }
#endif
}
