﻿using SimpleAVSGeneratorCore.Constants;
using SimpleAVSGeneratorCore.Support;

namespace SimpleAVSGeneratorCore.Tests.Support;

public class ExtensionsTests
{
    // FileExt | Expected FileType
    public static TheoryData<string, string> DetermineInputFileType_ValidateFileTypeIsCorrect_TestData =>
        new()
        {
            { ".mp4", FileExtensionTypes.Container },
            { ".264", FileExtensionTypes.Video     },
            { ".m4a", FileExtensionTypes.Audio     }
        };

    [Theory(DisplayName = "Validate That Input File Type Is Set Correctly")]
    [MemberData(nameof(DetermineInputFileType_ValidateFileTypeIsCorrect_TestData))]
    public async Task DetermineInputFileType_ValidateFileTypeIsCorrect(
        string fileExt,
        string expectedFileType)
    {
        // Arrange
        // Expected result is passed as a parameter

        // Act
        string actualFileType = await Extensions.DetermineInputFileTypeAsync(fileExt);

        // Assert
        Assert.Equal(expectedFileType, actualFileType);
    }

    // FileExt | Expected IsSupportedByMP4Box
    public static TheoryData<string, bool> IsSupportedByMP4Box_ValidateSupport_TestData =>
        new()
        {
            { ".mp4", true  },
            { ".mkv", false },
            { ".264", true  },
            { ".m4a", true  }
        };

    [Theory(DisplayName = "Validate That MP4Box Support Is Set Correctly")]
    [MemberData(nameof(IsSupportedByMP4Box_ValidateSupport_TestData))]
    public async Task IsSupportedByMP4Box_ValidateSupport(
        string fileExt,
        bool expectedIsSupportedByMP4Box)
    {
        // Arrange
        // Expected result is passed as a parameter

        // Act
        bool actualIsSupportedByMP4Box = await Extensions.IsSupportedByMP4BoxAsync(fileExt);

        // Assert
        Assert.Equal(expectedIsSupportedByMP4Box, actualIsSupportedByMP4Box);
    }

    [Theory(DisplayName = "Check Supported Extension Type Strings For Regex Pattern \"*.ext1;*.ext2\"")]
    [InlineData(FileExtensionTypes.Container)]
    [InlineData(FileExtensionTypes.Video)]
    [InlineData(FileExtensionTypes.Audio)]
    public async Task SupportedExtensionType_CheckPatternForMatch(string extensionType)
    {
        // Arrange
        // This pattern was a little difficult to set up
        // So lets break it down
        // The string must look like this "*.ext1;*.ext2;*.ext3" without the ';' at the end
        // A grouping can be seen, *.ext1 being a group
        // In regex, a group is inside round brackets ()
        // The * and the . are special characters in regex
        // but couldn't be escaped by the normal means e.g. \. \*
        // so it had to be in square brackets in the group
        // The pattern so far is ([*][.])
        // The next grouping is the characters that make the rest of the regex group,
        // alphanumeric characters a to z and 0 to 9 which is together [a-z0-9]
        // ([*][.][a-z0-9])
        // In the group, it ends in a semi-colon
        // The complete group ([*][.][a-z0-9];)
        // It appears continuously throughout the string so use the * to indicate that
        // ([*][.][a-z0-9];)*
        // Now the string cannot end with the semi-colon but that has to be in square brackets [;]
        // To ensure it doesn't, you have to use this character ^ to negate it [^;]
        // ([*][.][a-z0-9];)*[^;]
        // And finally, the end of string character $
        string expectedPattern = "([*][.][a-z0-9];)*[^;]$";

        // Act
        var extensions = new Extensions();
        await extensions.ConfigureSupportedExtensionsAsync();

        string? supportedExts = extensionType switch
        {
            FileExtensionTypes.Container => extensions.SupportedContainerExts,
            FileExtensionTypes.Video     => extensions.SupportedVideoExts,
            FileExtensionTypes.Audio     => extensions.SupportedAudioExts,
            _                                    => null
        };

        // Assert
        Assert.Matches(expectedPattern, supportedExts);
    }

    [Theory(DisplayName = "Check Filter Extension Type Strings For Regex Pattern \"EXT1 EXT2\"")]
    [InlineData(FileExtensionTypes.Container)]
    [InlineData(FileExtensionTypes.Video)]
    [InlineData(FileExtensionTypes.Audio)]
    public async Task FilterExtensionType_CheckPatternForMatch(string extensionType)
    {
        // Arrange
        // This pattern was a little similar to the previous but with a new challenge
        // \w is not the escape character for a whitespace
        // so I had to consult the documentation and found this \x020
        // This of all things is the escape character for a whitespace
        // So lets break it down
        // The string must look like this "EXT1 EXT2 EXT3" without the whitespace at the end
        // A grouping can be seen, 'EXT1\x020' being a group
        // In regex, a group is inside round brackets ()
        // The regex group consists of alphanumeric characters A to Z and 0 to 9
        // which is together in square brackets [A-Z0-9]
        // ([A-Z0-9])
        // In the regex group, it ends in a whitespace
        // The complete group ([A-Z0-9]\x020)
        // It appears continuously throughout the string so use the * to indicate that
        // ([A-Z0-9]\x020)*
        // Now the string cannot end with a whitespace but that has to be in square brackets [\x020]
        // To ensure it doesn't, you have to use this character ^ to negate it [^\x020]
        // ([A-Z0-9]\x020)*[^\x020]
        // And finally, the end of string character $
        string expectedPattern = "([A-Z0-9]\x020)*[^\x020]$";

        // Act
        var extensions = new Extensions();
        await extensions.ConfigureSupportedExtensionsAsync();

        string? filterExts = extensionType switch
        {
            FileExtensionTypes.Container => extensions.FilterContainerExts,
            FileExtensionTypes.Video     => extensions.FilterVideoExts,
            FileExtensionTypes.Audio     => extensions.FilterAudioExts,
            _                                    => null
        };

        // Assert
        Assert.Matches(expectedPattern, filterExts);
    }
}
