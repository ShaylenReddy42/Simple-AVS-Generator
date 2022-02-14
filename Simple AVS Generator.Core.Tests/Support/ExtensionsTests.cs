using System.Collections.Generic;
using Xunit;
using static Simple_AVS_Generator.Core.Enums;
using Simple_AVS_Generator.Core.Support;

namespace Simple_AVS_Generator.Core.Tests.Support
{
    public class ExtensionsTests
    {
        // FileExt | Expected FileType
        public static IEnumerable<object[]> DetermineInputFileType_ValidateFileTypeIsCorrect_TestData =
        new[]
        {
            new object[] { ".mp4", (int)ExtensionTypes.CONTAINER },
            new object[] { ".264", (int)ExtensionTypes.VIDEO     },
            new object[] { ".m4a", (int)ExtensionTypes.AUDIO     }
        };

        [Theory (DisplayName = "Validate That Input File Type Is Set Correctly")]
        [MemberData(nameof(DetermineInputFileType_ValidateFileTypeIsCorrect_TestData))]
        public void DetermineInputFileType_ValidateFileTypeIsCorrect
        (
            string fileExt,
            int expectedFileType
        )
        {
            // Arrange
            // Expected result is passed as a parameter

            // Act
            Extensions se = new();
            int actualFileType = se.DetermineInputFileType(fileExt);

            // Assert
            Assert.Equal(expectedFileType, actualFileType);
        }

        public static IEnumerable<object[]> IsSupportedByMP4Box_ValidateSupport_TestData =
        new[]
        {
            new object[]{ ".mp4", true  },
            new object[]{ ".mkv", false },
            new object[]{ ".264", true  },
            new object[]{ ".m4a", true  },
        };

        [Theory (DisplayName = "Validate That MP4Box Support Is Set Correctly")]
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
    }
}
