using System.Collections.Generic;
using Xunit;

using static SimpleAVSGeneratorCore.Support.Audio;

namespace SimpleAVSGeneratorCore.Tests.Support;

public class AudioTests
{
    [Fact(DisplayName = "Validate Audio Codecs")]
    public void GetOutputAudioCodecs_ValidateAudioCodecs()
    {
        // Arrange
        object[] expectedAudioCodecs = new object[] { "AAC-LC", "AAC-HE", "OPUS" };

        // Act
        object[] actualAudioCodecs = GetOutputAudioCodecs();

        // Assert
        Assert.Equal(expectedAudioCodecs, actualAudioCodecs);
    }

    [Fact(DisplayName = "Validate Languages")]
    public void GetLanguages_ValidateLanguages()
    {
        // Arrange
        object[] expectedLanguages = new object[] { "English", "Hindi", "Japanese", "Tamil", "Undetermined" };

        // Act
        object[] actualLanguages = GetLanguages();

        // Assert
        Assert.Equal(expectedLanguages, actualLanguages);
    }

    [Fact(DisplayName = "Validate Audio Channels")]
    public void GetAudioChannels_ValidateAudioChannels()
    {
        // Arrange
        object[] expectedAudioChannels = new object[] { "Stereo", "Surround 5.1", "Surround 7.1" };

        // Act
        object[] actualAudioChannels = GetAudioChannels();

        // Assert
        Assert.Equal(expectedAudioChannels, actualAudioChannels);
    }

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
