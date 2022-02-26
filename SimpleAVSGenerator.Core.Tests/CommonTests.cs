using System.Collections.Generic;
using Xunit;

namespace SimpleAVSGenerator.Core.Tests;

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

    [Theory (DisplayName = "Check If Properties Are Set Accurately")]
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
        Common common = new(fileName);
        string actualFileName     = common.FileName,
               actualFileExt      = common.FileExt,
               actualFileNameOnly = common.FileNameOnly,
               actualFileType = common.FileType;
        bool actualIsSupportedByMP4Box = common.IsSupportedByMP4Box;

        object[] actualProperties = { actualFileName, actualFileExt, actualFileNameOnly, actualFileType, actualIsSupportedByMP4Box };

        // Assert
        Assert.Equal(expectedProperties, actualProperties);
    }
}
