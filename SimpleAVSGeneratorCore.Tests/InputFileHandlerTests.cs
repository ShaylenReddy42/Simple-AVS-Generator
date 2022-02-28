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

namespace SimpleAVSGeneratorCore.Tests;

public class InputFileHandlerTests
{
#if DEBUG
    // Video | VideoCodec | Audio | OutputContainer | Expected scripts created
    public static IEnumerable<object?[]> CreateScripts_ValidateWhichScriptsWereCreated_TestData =
    new[]
    {
        new object?[] { true,  "HEVC",         true,  "MP4", "svac" },
        new object?[] { true,  "AV1",          false, "MP4", "svc"  },
        new object?[] { true,  "AVC",          false, null,  "sv"   },
        new object?[] { true,  "Mux Original", false, "MP4", "c"    },
        new object?[] { true,  "Mux Original", true,  "MP4", "sac"  },
        new object?[] { false, "",             true,  null,  "sa"   }
    };

    [Theory(DisplayName = "Validate Which Scripts Were Created")]
    [MemberData(nameof(CreateScripts_ValidateWhichScriptsWereCreated_TestData))]
    public void CreateScripts_ValidateWhichScriptsWereCreated
    (
        bool video,
        string videoCodec,
        bool audio,
        string? outputContainer,
        string expectedScriptsCreated
    )
    {
        // Arrange
        InputFileHandler input = new(@"C:\Users\User\Desktop\Sample.mp4");

        input.common.OutputDir = @"C:\Users\User\Desktop\Temp\Sample\";

        input.common.Video = video;
        input.common.VideoCodec = videoCodec;
        input.common.SourceFPS = 24;
        input.common.KeyframeIntervalInSeconds = 2;

        input.common.Audio = audio;
        input.common.AudioCodec = "AAC-LC";
        input.common.AudioBitrate = 128;
        input.common.AudioLanguage = "English";

        input.common.OutputContainer = outputContainer;

        // Act
        input.CreateScripts(out string actualScriptsCreated);

        // Assert
        Assert.Equal(expectedScriptsCreated, actualScriptsCreated);
    }
#endif
}
