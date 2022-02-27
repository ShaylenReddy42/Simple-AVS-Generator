/******************************************************************************
 * Copyright (C) 2022 Shaylen Reddy
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along
 * with this program; if not, write to the Free Software Foundation, Inc.,
 * 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
 ******************************************************************************/

using System.Collections.Generic;
using Xunit;

using SimpleAVSGeneratorCore.Support;

namespace SimpleAVSGeneratorCore.Tests.Support;

public class ExtensionsTests
{
    // FileExt | Expected FileType
    public static IEnumerable<object[]> DetermineInputFileType_ValidateFileTypeIsCorrect_TestData =
    new[]
    {
        new object[] { ".mp4", "CONTAINER" },
        new object[] { ".264", "VIDEO"     },
        new object[] { ".m4a", "AUDIO"     }
    };

    [Theory(DisplayName = "Validate That Input File Type Is Set Correctly")]
    [MemberData(nameof(DetermineInputFileType_ValidateFileTypeIsCorrect_TestData))]
    public void DetermineInputFileType_ValidateFileTypeIsCorrect
    (
        string fileExt,
        string expectedFileType
    )
    {
        // Arrange
        // Expected result is passed as a parameter

        // Act
        Extensions se = new();
        string actualFileType = se.DetermineInputFileType(fileExt);

        // Assert
        Assert.Equal(expectedFileType, actualFileType);
    }

    // FileExt | Expected IsSupportedByMP4Box
    public static IEnumerable<object[]> IsSupportedByMP4Box_ValidateSupport_TestData =
    new[]
    {
        new object[] { ".mp4", true  },
        new object[] { ".mkv", false },
        new object[] { ".264", true  },
        new object[] { ".m4a", true  }
    };

    [Theory(DisplayName = "Validate That MP4Box Support Is Set Correctly")]
    [MemberData(nameof(IsSupportedByMP4Box_ValidateSupport_TestData))]
    public void IsSupportedByMP4Box_ValidateSupport
    (
        string fileExt,
        bool expectedIsSupportedByMP4Box
    )
    {
        // Arrange
        // Expected result is passed as a parameter

        // Act
        Extensions se = new();
        bool actualIsSupportedByMP4Box = se.IsSupportedByMP4Box(fileExt);

        // Assert
        Assert.Equal(expectedIsSupportedByMP4Box, actualIsSupportedByMP4Box);
    }

    [Theory(DisplayName = "Check Supported Extension Type Strings For Regex Pattern \"*.ext1;*.ext2\"")]
    [InlineData("CONTAINER")]
    [InlineData("VIDEO")]
    [InlineData("AUDIO")]
    public void SupportedExtensionType_CheckPatternForMatch(string extensionType)
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
        Extensions se = new();
        string? supportedExts =
        extensionType switch
        {
            "CONTAINER" => se.SupportedContainerExts,
            "VIDEO"     => se.SupportedVideoExts,
            "AUDIO"     => se.SupportedAudioExts,
            _           => null
        };

        // Assert
        Assert.Matches(expectedPattern, supportedExts);
    }

    [Theory(DisplayName = "Check Filter Extension Type Strings For Regex Pattern \"EXT1 EXT2\"")]
    [InlineData("CONTAINER")]
    [InlineData("VIDEO")]
    [InlineData("AUDIO")]
    public void FilterExtensionType_CheckPatternForMatch(string extensionType)
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
        Extensions se = new();
        string? filterExts =
        extensionType switch
        {
            "CONTAINER" => se.FilterContainerExts,
            "VIDEO"     => se.FilterVideoExts,
            "AUDIO"     => se.FilterAudioExts,
            _           => null
        };

        // Assert
        Assert.Matches(expectedPattern, filterExts);
    }
}
