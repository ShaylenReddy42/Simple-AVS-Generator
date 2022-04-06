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
    public static IEnumerable<object[]> GetSelectableAndDefaultAudioBitrates_ValidateResult_TestData =
    new[]
    {
        new object[] { "AAC-LC", "2.0", new object[] {  96, 112, 128, 144, 160, 192 }, 128 },
        new object[] { "AAC-HE", "5.1", new object[] {  80,  96, 112, 128, 160, 192 }, 192 },
        new object[] { "OPUS",   "7.1", new object[] { 256, 288, 320, 384, 448, 576 }, 384 }
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
