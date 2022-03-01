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

public class CommonTests
{
    // FileName | FileExt | FileNameOnly | FileType | IsSupportedByMP4Box
    public static IEnumerable<object[]> Common_CheckIfPropertiesAreSetAccurately_TestData =>
    new[]
    {
        new object[] { @"C:\Users\User\Desktop\Sample1.mp4", ".mp4", "Sample1", "CONTAINER", true  },
        new object[] { @"C:\Users\User\Desktop\Sample2.mkv", ".mkv", "Sample2", "CONTAINER", false },
        new object[] { @"C:\Users\User\Desktop\Sample3.265", ".265", "Sample3", "VIDEO",     true  },
        new object[] { @"C:\Users\User\Desktop\Sample4.m4a", ".m4a", "Sample4", "AUDIO",     true  }
    };

    [Theory(DisplayName = "Check If Properties Are Set Accurately")]
    [MemberData(nameof(Common_CheckIfPropertiesAreSetAccurately_TestData))]
    public void Common_CheckIfPropertiesAreSetAccurately
    (
        string fileName,
        string fileExt,
        string fileNameOnly,
        string fileType,
        bool isSupportedByMP4Box
    )
    {
        // Arrange
        object[] expectedProperties = { fileName, fileExt, fileNameOnly, fileType, isSupportedByMP4Box };
        
        // Act
        Common common = new(fileName, @"C:\Users\User\Desktop\Temp\");
        string actualFileName     = common.FileName,
               actualFileExt      = common.FileExt,
               actualFileNameOnly = common.FileNameOnly,
               actualFileType     = common.FileType;
        bool actualIsSupportedByMP4Box = common.IsSupportedByMP4Box;

        object[] actualProperties = { actualFileName, actualFileExt, actualFileNameOnly, actualFileType, actualIsSupportedByMP4Box };

        // Assert
        Assert.Equal(expectedProperties, actualProperties);
    }
}
