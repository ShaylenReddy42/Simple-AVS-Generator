using SimpleAVSGeneratorCore.Constants;
using SimpleAVSGeneratorCore.Models;
using System.Text;

namespace SimpleAVSGeneratorCore.Support;

public class Extensions
{
    
    
    public string? SupportedContainerExts { get; private set; }
    public string? SupportedVideoExts { get; private set; }
    public string? SupportedAudioExts { get; private set; }

    public string? FilterContainerExts { get; private set; }
    public string? FilterVideoExts { get; private set; }
    public string? FilterAudioExts { get; private set; }

    private static readonly List<SupportedExtension> supportedExtensions =
    [
        /* -------- CONTAINER -------- */
        new SupportedExtension(extension: ".3gp", type: FileExtensionTypes.Container, mp4boxSupport: true),
        new SupportedExtension(".3g2",  FileExtensionTypes.Container, true),
        new SupportedExtension(".asf",  FileExtensionTypes.Container, false),
        new SupportedExtension(".avi",  FileExtensionTypes.Container, true),
        new SupportedExtension(".flv",  FileExtensionTypes.Container, false),
        new SupportedExtension(".mp4",  FileExtensionTypes.Container, true),
        new SupportedExtension(".m4v",  FileExtensionTypes.Container, true),
        new SupportedExtension(".mkv",  FileExtensionTypes.Container, false),
        new SupportedExtension(".mov",  FileExtensionTypes.Container, false),
        new SupportedExtension(".m2t",  FileExtensionTypes.Container, true),
        new SupportedExtension(".m2ts", FileExtensionTypes.Container, true),
        new SupportedExtension(".mxf",  FileExtensionTypes.Container, false),
        new SupportedExtension(".ogm",  FileExtensionTypes.Container, false),
        new SupportedExtension(".rm",   FileExtensionTypes.Container, false),
        new SupportedExtension(".rmvb", FileExtensionTypes.Container, false),
        new SupportedExtension(".ts",   FileExtensionTypes.Container, true),
        new SupportedExtension(".webm", FileExtensionTypes.Container, false),
        new SupportedExtension(".wmv",  FileExtensionTypes.Container, false),
        /* ---------- VIDEO ---------- */
        new SupportedExtension(extension: ".263", type: FileExtensionTypes.Video, mp4boxSupport: true),
        new SupportedExtension(".h263", FileExtensionTypes.Video, true),
        new SupportedExtension(".264",  FileExtensionTypes.Video, true),
        new SupportedExtension(".h264", FileExtensionTypes.Video, true),
        new SupportedExtension(".265",  FileExtensionTypes.Video, true),
        new SupportedExtension(".h265", FileExtensionTypes.Video, true),
        new SupportedExtension(".hevc", FileExtensionTypes.Video, true),
        new SupportedExtension(".ivf",  FileExtensionTypes.Video, true),
        new SupportedExtension(".obu",  FileExtensionTypes.Video, true),
        new SupportedExtension(".y4m",  FileExtensionTypes.Video, true),
        /* ---------- AUDIO ---------- */
        new SupportedExtension(extension: ".aa3", type: FileExtensionTypes.Audio, mp4boxSupport: false),
        new SupportedExtension(".aac",  FileExtensionTypes.Audio, true),
        new SupportedExtension(".aif",  FileExtensionTypes.Audio, false),
        new SupportedExtension(".ac3",  FileExtensionTypes.Audio, true),
        new SupportedExtension(".ape",  FileExtensionTypes.Audio, false),
        new SupportedExtension(".dts",  FileExtensionTypes.Audio, false),
        new SupportedExtension(".flac", FileExtensionTypes.Audio, true),
        new SupportedExtension(".m1a",  FileExtensionTypes.Audio, true),
        new SupportedExtension(".m2a",  FileExtensionTypes.Audio, true),
        new SupportedExtension(".mp2",  FileExtensionTypes.Audio, true),
        new SupportedExtension(".mp3",  FileExtensionTypes.Audio, true),
        new SupportedExtension(".m4a",  FileExtensionTypes.Audio, true),
        new SupportedExtension(".oma",  FileExtensionTypes.Audio, false),
        new SupportedExtension(".opus", FileExtensionTypes.Audio, true),
        new SupportedExtension(".thd",  FileExtensionTypes.Audio, false),
        new SupportedExtension(".tta",  FileExtensionTypes.Audio, false),
        new SupportedExtension(".wav",  FileExtensionTypes.Audio, true),
        new SupportedExtension(".wma",  FileExtensionTypes.Audio, false)
    ];

    private Task SetSupportForAsync(string fileType)
    {
        StringBuilder sbSupport = new();

        supportedExtensions
            .Where(ext => ext.Type == fileType)
            .ToList()
            .ForEach(ext => sbSupport.Append($"*{ext.Extension};"));

        string support = sbSupport.ToString()[0..^1];

        switch (fileType)
        {
            case FileExtensionTypes.Container:
                SupportedContainerExts = support;
                break;
            case FileExtensionTypes.Video:
                SupportedVideoExts = support;
                break;
            case FileExtensionTypes.Audio:
                SupportedAudioExts = support;
                break;
        }

        return Task.CompletedTask;
    }

    private Task SetFilterForAsync(string fileType)
    {
        StringBuilder sbFilter = new();

        supportedExtensions
            .Where(ext => ext.Type == fileType)
            .ToList()
            .ForEach(ext => sbFilter.Append($"{ext.Extension[1..].ToUpper()} "));

        string filter = sbFilter.ToString()[0..^1];

        switch (fileType)
        {
            case FileExtensionTypes.Container:
                FilterContainerExts = filter;
                break;
            case FileExtensionTypes.Video:
                FilterVideoExts = filter;
                break;
            case FileExtensionTypes.Audio:
                FilterAudioExts = filter;
                break;
        }

        return Task.CompletedTask;
    }

    public static Task<string> DetermineInputFileTypeAsync(string fileExt) =>
        Task.FromResult(supportedExtensions.Single(ext => ext.Extension == fileExt).Type);

    public static Task<bool> IsSupportedByMP4BoxAsync(string fileExt) =>
        Task.FromResult(supportedExtensions.Single(ext => ext.Extension == fileExt).MP4BoxSupport);

    public async Task ConfigureSupportedExtensionsAsync()
    {
        await SetSupportForAsync(FileExtensionTypes.Container);
        await SetSupportForAsync(FileExtensionTypes.Video);
        await SetSupportForAsync(FileExtensionTypes.Audio);

        await SetFilterForAsync(FileExtensionTypes.Container);
        await SetFilterForAsync(FileExtensionTypes.Video);
        await SetFilterForAsync(FileExtensionTypes.Audio);
    }
}
