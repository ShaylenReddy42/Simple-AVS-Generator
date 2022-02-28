using System.Collections.Generic;
using Xunit;

using static SimpleAVSGeneratorCore.Support.Audio;

namespace SimpleAVSGeneratorCore.Tests.Support;

public class AudioTests
{
    // AudioCodec | AudioChannels | Expected audio bitrates | Expected default audio bitrate
    public static IEnumerable<object[]> GetSelectableAndDefaultAudioBitrates_ValidateResult_TestData =
    new[]
    {
        new object[] { "AAC-LC", "Stereo",       new object[] {  96, 112, 128, 144, 160, 192 }, 128 },
        new object[] { "AAC-HE", "Surround 5.1", new object[] {  80,  96, 112, 128, 160, 192 }, 192 },
        new object[] { "OPUS",   "Surround 7.1", new object[] { 256, 288, 320, 384, 448, 576 }, 384 }
    };

    [Theory(DisplayName = "Validate Whether Returned Audio Bitrates And Default Is Correct")]
    [MemberData(nameof(GetSelectableAndDefaultAudioBitrates_ValidateResult_TestData))]
    public void GetSelectableAndDefaultAudioBitrates_ValidateResult
    (
        string audioCodec,
        string audioChannels,
        object[] expectedAudioBitrates,
        int expectedDefaultAudioBitrate
    )
    {
        // Arrange
        object[] expected = new object[] { expectedAudioBitrates, expectedDefaultAudioBitrate };

        // Act
        (object[] actualAudioBitrates, int actualDefaultAudioBitrate) = GetSelectableAndDefaultAudioBitrates(audioCodec, audioChannels);

        object[] actual = new object[] { actualAudioBitrates, actualDefaultAudioBitrate };
        
        // Assert
        Assert.Equal(expected, actual);
    }
}
