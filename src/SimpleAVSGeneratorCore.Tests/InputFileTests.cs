namespace SimpleAVSGeneratorCore.Tests;

public class InputFileTests
{
    // FileName | FileExt | FileNameOnly | FileType | IsSupportedByMP4Box
    public static TheoryData<string, string, string, string, bool> InputFile_CheckIfPropertiesAreSetAccurately_TestData =>
        new()
        {
            { @"Samples\Sample.mp4",  ".mp4", "Sample",  "CONTAINER", true  },
            { @"Samples\Sample1.mkv", ".mkv", "Sample1", "CONTAINER", false },
            { @"Samples\Sample.265",  ".265", "Sample",  "VIDEO",     true  },
            { @"Samples\Sample.m4a",  ".m4a", "Sample",  "AUDIO",     true  }
        };

    [Theory(DisplayName = "Check If Properties Are Set Accurately")]
    [MemberData(nameof(InputFile_CheckIfPropertiesAreSetAccurately_TestData))]
    public void InputFile_CheckIfPropertiesAreSetAccurately(
        string fileName,
        string fileExt,
        string fileNameOnly,
        string fileType,
        bool isSupportedByMP4Box)
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
    public void ValidateAVSMeterScriptFileAndContent(
        string fileName,
        string expectedScriptFile)
    {
        // Arrange
        string[] expectedScriptFileAndContent = { expectedScriptFile, $"AVSMeter64 \"%~dp0Script.avs\" -i -l" };

        InputFile input = new(fileName, @"C:\Users\User\Desktop\Temp\");

        // Act
        string actualScriptFile    = input.AVSMeterScriptFile,
               actualScriptContent = InputFile.AVSMeterScriptContent;

        string[] actualScriptFileAndContent = { actualScriptFile, actualScriptContent };

        // Assert
        Assert.Equal(expectedScriptFileAndContent, actualScriptFileAndContent);
    }

    [Theory(DisplayName = "Validate Simple Audio Channel Layout")]
    // FileName | Expected audio channel layout
    [InlineData(@"Samples\Sample.mp3",       "2.0")]
    [InlineData(@"Samples\Sample.m4a",       "2.0")]
    [InlineData(@"Samples\Sample DTS_X.mkv", "7.1")] // This is an 8.0 track mapped to 7.1
    public void ValidateSimpleAudioChannelLayout(
        string fileName,
        string expectedAudioChannelLayout)
    {
        // Arrange
        // Nothing to do here

        // Act
        InputFile input = new(fileName, @"C:\Users\User\Desktop\Temp\");

        // Assert
        Assert.Equal(expectedAudioChannelLayout, input.Audio.SourceChannels);
    }

    // Video | VideoCodec | Audio | OutputContainer | Expected scripts created
    public static TheoryData<bool, string, bool, string?, string> CreateScripts_ValidateWhichScriptsWereCreated_TestData =>
        new()
        {
            { true,  "HEVC",         true,  "MP4", "svac" },
            { true,  "AV1",          false, "MP4", "svc"  },
            { true,  "AVC",          false, null,  "sv"   },
            { true,  "Mux Original", false, "MP4", "c"    },
            { true,  "Mux Original", true,  "MP4", "sac"  },
            { false, "",             true,  null,  "sa"   },
            { false, "HEVC",         false, "MP4", ""     },
            { false, "Mux Original", false, "MKV", ""     }

        };

    [Theory(DisplayName = "Validate Which Scripts Were Created")]
    [MemberData(nameof(CreateScripts_ValidateWhichScriptsWereCreated_TestData))]
    public async Task CreateScripts_ValidateWhichScriptsWereCreated(
        bool video,
        string videoCodec,
        bool audio,
        string? outputContainer,
        string expectedScriptsCreated)
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
        var actualScriptsCreated = await input.CreateScriptsAsync();

        // Assert
        Assert.Equal(expectedScriptsCreated, actualScriptsCreated);
    }
}
