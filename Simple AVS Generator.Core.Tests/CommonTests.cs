using System.Collections.Generic;
using Xunit;
using static Simple_AVS_Generator.Core.Enums;

namespace Simple_AVS_Generator.Core.Tests
{
    public class CommonTests
    {
        // FileName | FileExt | FileNameOnly | FileType | IsSupportedByMP4Box
        public static IEnumerable<object[]> Common_CheckIfPropertiesAreSetAccurately_TestData =>
        new[]
        {
            new object[] { @"C:\Users\User\Desktop\Sample1.mp4", ".mp4", "Sample1", (int)ExtensionTypes.CONTAINER, true  },
            new object[] { @"C:\Users\User\Desktop\Sample2.mkv", ".mkv", "Sample2", (int)ExtensionTypes.CONTAINER, false },
            new object[] { @"C:\Users\User\Desktop\Sample3.265", ".265", "Sample3", (int)ExtensionTypes.VIDEO, true      },
            new object[] { @"C:\Users\User\Desktop\Sample4.m4a", ".m4a", "Sample4", (int)ExtensionTypes.AUDIO, false     }
        };

        // COMMENT
        // Since audio files are not checked for mp4box support, false is always returned even if it is supported
        // Logic shouldn't change but caught onto this via running the tests with true for the IsSupportedByMP4Box property
        // If the logic should reflect this, it means altering the array that holds all the supported extensions to include audio
        
        [Theory (DisplayName = "Check If Properties Are Set Accurately")]
        [MemberData(nameof(Common_CheckIfPropertiesAreSetAccurately_TestData))]
        public void Common_CheckIfPropertiesAreSetAccurately(string fileName, string fileExt, string fileNameOnly, int fileType, bool isSupportedByMP4Box)
        {
            // Arrange
            object[] expectedProperties = { fileName, fileExt, fileNameOnly, fileType, isSupportedByMP4Box };
            
            // Act
            Common common = new(fileName);
            string actualFileName     = common.FileName,
                   actualFileExt      = common.FileExt,
                   actualFileNameOnly = common.FileNameOnly;
            int actualFileType = common.FileType;
            bool actualIsSupportedByMP4Box = common.IsSupportedByMP4Box;

            object[] actualProperties = { actualFileName, actualFileExt, actualFileNameOnly, actualFileType, actualIsSupportedByMP4Box };

            // Assert
            Assert.Equal(expectedProperties, actualProperties);
        }
    }
}
