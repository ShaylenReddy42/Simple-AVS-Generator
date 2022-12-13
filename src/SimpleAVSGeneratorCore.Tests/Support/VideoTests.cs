using Xunit;

using static SimpleAVSGeneratorCore.Support.Video;

namespace SimpleAVSGeneratorCore.Tests.Support;

public class VideoTests
{
    [Fact(DisplayName = "Validate Video Codecs")]
    public void GetOutputVideoCodecs_ValidateVideoCodecs()
    {
        // Arrange
        object[] expectedVideoCodecs = new object[] { "HEVC", "AV1", "AVC", "WhatsApp", "Mux Original" };

        // Act
        object[] actualVideoCodecs = GetOutputVideoCodecs();

        // Assert
        Assert.Equal(expectedVideoCodecs, actualVideoCodecs);
    }

    [Fact(DisplayName = "Validate Keyframe Intervals In Seconds")]
    public void GetKeyframeIntervals_ValidateKeyframeIntervalsInSeconds()
    {
        // Arrange
        object[] expectedKeyframeIntervals = new object[] { "2 Seconds", "5 Seconds", "10 Seconds" };

        // Act
        object[] actualKeyframeIntervals = GetKeyframeIntervals();

        // Assert
        Assert.Equal(expectedKeyframeIntervals, actualKeyframeIntervals);
    }
}
