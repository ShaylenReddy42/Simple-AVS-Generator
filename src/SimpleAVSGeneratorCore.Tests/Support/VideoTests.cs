using static SimpleAVSGeneratorCore.Support.Video;

namespace SimpleAVSGeneratorCore.Tests.Support;

public class VideoTests
{
    [Fact(DisplayName = "Validate Video Codecs")]
    public async Task GetOutputVideoCodecs_ValidateVideoCodecs()
    {
        // Arrange
        object[] expectedVideoCodecs = new object[] { "HEVC", "AV1", "AVC", "WhatsApp", "Mux Original" };

        // Act
        object[] actualVideoCodecs = await GetOutputVideoCodecsAsync();

        // Assert
        Assert.Equal(expectedVideoCodecs, actualVideoCodecs);
    }

    [Fact(DisplayName = "Validate Keyframe Intervals In Seconds")]
    public async Task GetKeyframeIntervals_ValidateKeyframeIntervalsInSeconds()
    {
        // Arrange
        object[] expectedKeyframeIntervals = new object[] { "2 Seconds", "5 Seconds", "10 Seconds" };

        // Act
        object[] actualKeyframeIntervals = await GetKeyframeIntervalsAsync();

        // Assert
        Assert.Equal(expectedKeyframeIntervals, actualKeyframeIntervals);
    }
}
