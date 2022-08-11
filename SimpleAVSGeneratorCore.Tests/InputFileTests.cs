using System.Collections.Generic;
using Xunit;

namespace SimpleAVSGeneratorCore.Tests;

public class InputFileTests
{
    // FileName | FileExt | FileNameOnly | FileType | IsSupportedByMP4Box
    public static readonly IEnumerable<object[]> InputFile_CheckIfPropertiesAreSetAccurately_TestData =
    new[]
    {
        new object[] { @"Samples\Sample.mp4",  ".mp4", "Sample",  "CONTAINER", true  },
        new object[] { @"Samples\Sample1.mkv", ".mkv", "Sample1", "CONTAINER", false },
        new object[] { @"Samples\Sample.265",  ".265", "Sample",  "VIDEO",     true  },
        new object[] { @"Samples\Sample.m4a",  ".m4a", "Sample",  "AUDIO",     true  }
    };

    [Theory(DisplayName = "Check If Properties Are Set Accurately")]
    [MemberData(nameof(InputFile_CheckIfPropertiesAreSetAccurately_TestData))]
    public void InputFile_CheckIfPropertiesAreSetAccurately
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
        InputFile input = new(fileName, @"C:\Users\User\Desktop\Temp\");
        string actualFileName     = input.FileInfo.FileName,
               actualFileExt      = input.FileInfo.FileExt,
               actualFileNameOnly = input.FileInfo.FileNameOnly,
               actualFileType     = input.FileInfo.FileType;
        bool actualIsSupportedByMP4Box = input.FileInfo.IsSupportedByMP4Box;

        object[] actualProperties = { actualFileName, actualFileExt, actualFileNameOnly, actualFileType, actualIsSupportedByMP4Box };

        // Assert
        Assert.Equal(expectedProperties, actualProperties);
    }

    [Theory(DisplayName = "Validate AVSMeter Script File And Content")]
    // FileName | Expected AVSMeter script file
    [InlineData(@"Samples\Sample.mp4",  @"C:\Users\User\Desktop\Temp\Sample\AVSMeter.cmd")]
    [InlineData(@"Samples\Sample1.mkv", @"C:\Users\User\Desktop\Temp\Sample1\AVSMeter.cmd")]
    public void ValidateAVSMeterScriptFileAndContent
    (
        string fileName,
        string expectedScriptFile
    )
    {
        // Arrange
        string[] expectedScriptFileAndContent = { expectedScriptFile, $"AVSMeter64 \"%~dp0Script.avs\" -i -l" };

        InputFile input = new(fileName, @"C:\Users\User\Desktop\Temp\");

        // Act
        string actualScriptFile    = input.AVSMeterScriptFile,
               actualScriptContent = input.AVSMeterScriptContent;

        string[] actualScriptFileAndContent = { actualScriptFile, actualScriptContent };

        // Assert
        Assert.Equal(expectedScriptFileAndContent, actualScriptFileAndContent);
    }

    [Theory(DisplayName = "Validate Simple Audio Channel Layout")]
    // FileName | Expected audio channel layout
    [InlineData(@"Samples\Sample.mp3",       "2.0")]
    [InlineData(@"Samples\Sample.m4a",       "2.0")]
    [InlineData(@"Samples\Sample DTS_X.mkv", "7.1")] // This is an 8.0 track mapped to 7.1
    public void ValidateSimpleAudioChannelLayout
    (
        string fileName,
        string expectedAudioChannelLayout
    )
    {
        // Arrange
        // Nothing to do here

        // Act
        InputFile input = new(fileName, @"C:\Users\User\Desktop\Temp\");

        // Assert
        Assert.Equal(expectedAudioChannelLayout, input.Audio.SourceChannels);
    }

    // Video | VideoCodec | Audio | OutputContainer | Expected scripts created
    public static readonly IEnumerable<object?[]> CreateScripts_ValidateWhichScriptsWereCreated_TestData =
    new[]
    {
        new object?[] { true,  "HEVC",         true,  "MP4", "svac" },
        new object?[] { true,  "AV1",          false, "MP4", "svc"  },
        new object?[] { true,  "AVC",          false, null,  "sv"   },
        new object?[] { true,  "Mux Original", false, "MP4", "c"    },
        new object?[] { true,  "Mux Original", true,  "MP4", "sac"  },
        new object?[] { false, "",             true,  null,  "sa"   },
        new object?[] { false, "HEVC",         false, "MP4", ""     },
        new object?[] { false, "Mux Original", false, "MKV", ""     }

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
        InputFile input = new(@"Samples\Sample.mp4", @"C:\Users\User\Desktop\Temp\");

        input.Video.Enabled = video;
        input.Video.Codec = videoCodec;
        input.Video.KeyframeIntervalInSeconds = "2 Seconds";

        input.Audio.Enabled = audio;
        input.Audio.Codec = "AAC-LC";
        input.Audio.Bitrate = 128;
        input.Audio.Language = "English";

        input.OutputContainer = outputContainer;

        // Act
        input.CreateScripts(out string actualScriptsCreated);

        // Assert
        Assert.Equal(expectedScriptsCreated, actualScriptsCreated);
    }
}
