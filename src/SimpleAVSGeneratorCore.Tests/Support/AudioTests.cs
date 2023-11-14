using static SimpleAVSGeneratorCore.Support.Audio;

namespace SimpleAVSGeneratorCore.Tests.Support;

public class AudioTests
{
    [Fact(DisplayName = "Validate Audio Codecs")]
    public async Task GetOutputAudioCodecs_ValidateAudioCodecs()
    {
        // Arrange
        var expectedAudioCodecs = new object[] { "AAC-LC", "AAC-HE", "OPUS" };

        // Act
        var actualAudioCodecs = await GetOutputAudioCodecsAsync();

        // Assert
        Assert.Equal(expectedAudioCodecs, actualAudioCodecs);
    }

    [Fact(DisplayName = "Validate Languages")]
    public async Task GetLanguages_ValidateLanguages()
    {
        // Arrange
        var expectedLanguages = new object[] { "English", "Hindi", "Japanese", "Tamil", "Undetermined" };

        // Act
        var actualLanguages = await GetLanguagesAsync();

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
    public async Task GetSelectableAndDefaultAudioBitrates_ValidateResult(
        string audioCodec,
        string audioChannels,
        object[] expectedAudioBitrates,
        int expectedDefaultAudioBitrate)
    {
        // Arrange
        var expected = new object[] { expectedAudioBitrates, expectedDefaultAudioBitrate };

        // Act
        (object[] actualAudioBitrates, int actualDefaultAudioBitrate) = await GetSelectableAndDefaultAudioBitratesAsync(audioCodec, audioChannels);

        var actual = new object[] { actualAudioBitrates, actualDefaultAudioBitrate };
        
        // Assert
        Assert.Equal(expected, actual);
    }
}
