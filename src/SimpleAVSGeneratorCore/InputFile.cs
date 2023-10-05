using MediaInfo;
using System.Globalization;
using SimpleAVSGeneratorCore.Models;
using SimpleAVSGeneratorCore.Support;

namespace SimpleAVSGeneratorCore;

public class InputFile
{
    public FileModel FileInfo { get; }

    public string HomeDir { get; set; }
    public string OutputDir => Path.Combine(HomeDir, FileInfo.FileNameOnly);

    public string ScriptFile => Path.Combine(OutputDir, "Script.avs");

    //AVSMeter Properties
    public string AVSMeterScriptFile => Path.Combine(OutputDir, "AVSMeter.cmd");
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
            FileType = Extensions.DetermineInputFileTypeAsync(fileExt).GetAwaiter().GetResult(),
            IsSupportedByMP4Box = Extensions.IsSupportedByMP4BoxAsync(fileExt).GetAwaiter().GetResult(),
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
                SourceChannels = GetSimpleAudioChannelLayoutAsync().GetAwaiter().GetResult()
            },
            false => new()
            {
                SourceChannels = "2.0"
            }
        };
    }

    private Task<string> GetSimpleAudioChannelLayoutAsync()
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
            "8.0" => Task.FromResult("7.1"),
            _     => Task.FromResult(channelLayout)
        };
    }
}
