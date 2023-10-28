using MediaInfo;
using Microsoft.Extensions.DependencyInjection;
using SimpleAVSGeneratorCore.Models;
using SimpleAVSGeneratorCore.Support;
using System.Globalization;

namespace SimpleAVSGeneratorCore.Services;

public class InputFileHandlerService : IInputFileHandlerService
{
    private readonly IFileWriterService fileWriterService;
    private readonly IServiceProvider serviceProvider;

    public InputFileHandlerService(
        IFileWriterService fileWriterService,
        IServiceProvider serviceProvider)
    {
        this.fileWriterService = fileWriterService;
        this.serviceProvider = serviceProvider;
    }

    public async Task<InputFile> CreateInputFileAsync(string fileName, string home)
    {
        var inputFile = new InputFile
        {
            HomeDir = home
        };

        using var scope = serviceProvider.CreateScope();

        var mediaInfo = scope.ServiceProvider.GetRequiredService<MediaInfo.MediaInfo>();

        mediaInfo.Open(fileName);

        string fileExtension = Path.GetExtension(fileName);

        var inputFileInfo = new FileModel
        {
            FileName = fileName,
            FileType = await Extensions.DetermineInputFileTypeAsync(fileExtension),
            IsSupportedByMP4Box = await Extensions.IsSupportedByMP4BoxAsync(fileExtension),
            HasVideo = mediaInfo.Get(StreamKind.General, 0, "VideoCount") is not "",
            HasAudio = mediaInfo.Get(StreamKind.General, 0, "AudioCount") is not ""
        };

        inputFile.FileInfo = inputFileInfo;

        var inputFileVideo = inputFile.FileInfo.HasVideo switch
        {
            true  => new VideoModel
            {
                SourceFPS = decimal.Parse(mediaInfo.Get(StreamKind.Video, 0, "FrameRate"), CultureInfo.InvariantCulture),
                SourceFrameCount = inputFile.FileInfo.FileType is "CONTAINER" ? int.Parse(mediaInfo.Get(StreamKind.Video, 0, "FrameCount")) : 0
            },
            false => new VideoModel
            {
                SourceFPS = 23.976M,
                SourceFrameCount = 0
            }
        };

        var inputFileAudio = inputFile.FileInfo.HasAudio switch
        {
            true  => new AudioModel
            {
                SourceChannels = await GetSimpleAudioChannelLayoutAsync(mediaInfo)
            },
            false => new AudioModel
            {
                SourceChannels = "2.0"
            }
        };

        inputFile.Video = inputFileVideo;

        inputFile.Audio = inputFileAudio;

        mediaInfo.Close();

        return inputFile;
    }

    public async Task<string> CreateScriptsAsync(InputFile inputFile)
    {
        // scriptsCreated is a variable that will be used for testing this function
        // Result could be in variable length, containing characters to indicated
        // which scripts were created e.g. svac
        // Key:
        // s indicates that the AviSynth script is created
        // v indicates that the Video Encoder script is created
        // a indicates that the Audio Encoder script is created
        // c indicates that the Container Muxing script is created

        var scriptsCreated = string.Empty;

#if RELEASE
        Directory.CreateDirectory(inputFile.OutputDir);
#endif

        using var scope = serviceProvider.CreateScope();

        var aviSynthScriptService = scope.ServiceProvider.GetRequiredService<AviSynthScriptService>();
        var outputScriptsService = scope.ServiceProvider.GetRequiredService<OutputScriptsService>();

        await aviSynthScriptService.SetScriptContentAsync(inputFile.ScriptFile, inputFile.FileInfo, inputFile.Video, inputFile.Audio);
        if (aviSynthScriptService.CreateAviSynthScript)
        {
            scriptsCreated += "s";

            await fileWriterService.WriteFileAsync(aviSynthScriptService.AVSScriptFile, aviSynthScriptService.AVSScriptContent);

            await fileWriterService.WriteFileAsync(inputFile.AVSMeterScriptFile, InputFile.AVSMeterScriptContent);
        }

        await outputScriptsService.ConfigureVideoScriptAsync(inputFile.Video, inputFile.OutputDir);
        if (outputScriptsService.VideoEncoderScriptFile is not null && outputScriptsService.VideoEncoderScriptContent is not null)
        {
            scriptsCreated += "v";

            await fileWriterService.WriteFileAsync(outputScriptsService.VideoEncoderScriptFile, outputScriptsService.VideoEncoderScriptContent);
        }

        await outputScriptsService.ConfigureAudioScriptAsync(inputFile.FileInfo, inputFile.Audio, inputFile.OutputDir);
        if (outputScriptsService.AudioEncoderScriptFile is not null && outputScriptsService.AudioEncoderScriptContent is not null)
        {
            scriptsCreated += "a";

            await fileWriterService.WriteFileAsync(outputScriptsService.AudioEncoderScriptFile, outputScriptsService.AudioEncoderScriptContent);

        }

        await outputScriptsService.ConfigureContainerScriptAsync(inputFile.FileInfo, inputFile.Video, inputFile.Audio, inputFile.OutputContainer, inputFile.OutputDir);
        if (outputScriptsService.ContainerScriptFile is not null && outputScriptsService.ContainerScriptContent is not null)
        {
            scriptsCreated += "c";

            await fileWriterService.WriteFileAsync(outputScriptsService.ContainerScriptFile, outputScriptsService.ContainerScriptContent);
        }

#if RELEASE
        if (scriptsCreated is "")
        {
            Directory.Delete(inputFile.OutputDir);
        }
#endif

        return scriptsCreated;
    }

    private static Task<string> GetSimpleAudioChannelLayoutAsync(MediaInfo.MediaInfo mediaInfo)
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
            _ => Task.FromResult(channelLayout)
        };
    }
}
