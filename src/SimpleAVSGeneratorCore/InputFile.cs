using System.Globalization;
using SimpleAVSGeneratorCore.Models;
using SimpleAVSGeneratorCore.Support;
using MediaInfo;

namespace SimpleAVSGeneratorCore;

public class InputFile
{
    public FileModel FileInfo { get; }

    public string HomeDir { get; set; }
    public string OutputDir => $@"{HomeDir}{FileInfo.FileNameOnly}\";

    public string ScriptFile => $@"{OutputDir}Script.avs";

    //AVSMeter Properties
    public string AVSMeterScriptFile => $"{OutputDir}AVSMeter.cmd";
    public static string AVSMeterScriptContent => @"AVSMeter64 ""%~dp0Script.avs"" -i -l";

    public VideoModel Video { get; }

    public AudioModel Audio { get; }

    public string? OutputContainer { get; set; }

    private readonly MediaInfo.MediaInfo mediaInfo = new();

    public InputFile(string fileName, string home)
    {
        HomeDir = home;

        mediaInfo.Open(fileName);

        string fileExt = Path.GetExtension(fileName);

        FileInfo = new()
        {
            FileName = fileName,
            FileType = Extensions.DetermineInputFileType(fileExt),
            IsSupportedByMP4Box = Extensions.IsSupportedByMP4Box(fileExt),
            HasVideo = mediaInfo.Get(StreamKind.General, 0, "VideoCount") is not "",
            HasAudio = mediaInfo.Get(StreamKind.General, 0, "AudioCount") is not ""
        };
        
        Video = FileInfo.HasVideo switch
        {
            true  => new()
            {
                SourceFPS = decimal.Parse(mediaInfo.Get(StreamKind.Video, 0, "FrameRate"), CultureInfo.InvariantCulture),
                SourceFrameCount = FileInfo.FileType is "CONTAINER" ? int.Parse(mediaInfo.Get(StreamKind.Video, 0, "FrameCount")) : 0
            },
            false => new()
            {
                SourceFPS = 23.976M,
                SourceFrameCount = 0
            }
        };

        Audio = FileInfo.HasAudio switch
        {
            true  => new()
            {
                SourceChannels = GetSimpleAudioChannelLayout()
            },
            false => new()
            {
                SourceChannels = "2.0"
            }
        };
    }

    private string GetSimpleAudioChannelLayout()
    {
        // Ensures the value from MediaInfo e.g. 2/0/0 or 3/2/0.1 or 3/2/2.1 returns 2.0, 5.1 or 7.1 respectively
        // Those are channel positions (Front/Side/Rear+LFE)

        string channelPositions = mediaInfo.Get(StreamKind.Audio, 0, "ChannelPositions/String2");
        channelPositions = channelPositions is "" ? mediaInfo.Get(StreamKind.Audio, 0, "Channels") : channelPositions;

        string channelLayout = 
            channelPositions.Split('/')
                .Sum(ch => double.Parse(ch, CultureInfo.InvariantCulture))
                .ToString("0.0", CultureInfo.InvariantCulture);

        // DTS:X is an 8 Channel Object-Based track
        // Ran into this bug and went down a rabbit hole
        // AAC-HE has to force a different channel mask
        // to even support 7.1 [0xff or 3/4/0.1]
        // instead of [0x63f or 3/2/2.1]
        // These hex values are from the research
        // 8.0 will be treated like 7.1
        // AAC-LC and OPUS creates a 7.1 [3/2/2.1]
        // track from the 8.0 without tampering
        return channelLayout switch
        {
            "8.0" => "7.1",
            _     => channelLayout
        };
    }

#if RELEASE
    private void WriteFile(string outputFileName, string fileContents)
    {
        StreamWriter sw = new StreamWriter(outputFileName);
        sw.Write($"{(outputFileName.EndsWith(".cmd") ? "@ECHO off\r\n\r\n" : "")}{fileContents}");
        sw.Close();
    }
#endif

    public void CreateScripts(out string scriptsCreated)
    {
        // scriptsCreated is a variable that will be used for testing this function
        // Result could be in variable length, containing characters to indicated
        // which scripts were created e.g. svac
        // Key:
        // s indicates that the AviSynth script is created
        // v indicates that the Video Encoder script is created
        // a indicates that the Audio Encoder script is created
        // c indicates that the Container Muxing script is created
        
        scriptsCreated = "";

#if RELEASE
        Directory.CreateDirectory(OutputDir);
#endif

        AviSynthScript script = new(ScriptFile);
        
        script.SetScriptContent(FileInfo, Video, Audio);
        if (script.CreateAviSynthScript is true)
        {
            scriptsCreated += "s";
#if RELEASE
            WriteFile(script.AVSScriptFile, script.AVSScriptContent);

            WriteFile(AVSMeterScriptFile, AVSMeterScriptContent);
#endif
        }

        OutputScripts output = new();

        output.ConfigureVideoScript(Video, OutputDir);
        if (output.VideoEncoderScriptFile is not null && output.VideoEncoderScriptContent is not null)
        {
            scriptsCreated += "v";
#if RELEASE
            WriteFile(output.VideoEncoderScriptFile, output.VideoEncoderScriptContent);
#endif
        }

        output.ConfigureAudioScript(FileInfo, Audio, OutputDir);
        if (output.AudioEncoderScriptFile is not null && output.AudioEncoderScriptContent is not null)
        {
            scriptsCreated += "a";
#if RELEASE
            WriteFile(output.AudioEncoderScriptFile, output.AudioEncoderScriptContent);
#endif
        }

        output.ConfigureContainerScript(FileInfo, Video, Audio, OutputContainer, OutputDir);
        if (output.ContainerScriptFile is not null && output.ContainerScriptContent is not null)
        {
            scriptsCreated += "c";
#if RELEASE
            WriteFile(output.ContainerScriptFile, output.ContainerScriptContent);
#endif
        }

#if RELEASE
        if (scriptsCreated is "")
        {
            Directory.Delete(OutputDir);
        }
#endif
    }
}
