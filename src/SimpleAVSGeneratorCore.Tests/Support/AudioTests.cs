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

    // AudioCodec | AudioChannels | Expected audio bitrates | Expected default audio bitrate
    public static TheoryData<string, string, object[], int> GetSelectableAndDefaultAudioBitrates_ValidateResult_TestData =>
        new()
        {
            { "AAC-LC", "2.0", new object[] {  96, 112, 128, 144, 160, 192 }, 128 },
            { "AAC-HE", "5.1", new object[] {  80,  96, 112, 128, 160, 192 }, 192 },
            { "OPUS",   "7.1", new object[] { 256, 288, 320, 384, 448, 512 }, 320 }
        };

    [Theory(DisplayName = "Validate Whether Returned Audio Bitrates And Default Is Correct")]
    [MemberData(nameof(GetSelectableAndDefaultAudioBitrates_ValidateResult_TestData))]
    public void GetSelectableAndDefaultAudioBitrates_ValidateResult(
        string audioCodec,
        string audioChannels,
        object[] expectedAudioBitrates,
        int expectedDefaultAudioBitrate)
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
