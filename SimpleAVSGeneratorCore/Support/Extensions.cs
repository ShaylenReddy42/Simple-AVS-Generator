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

    private static readonly List<SupportedExtension> supportedExtensions = new()
    {
        /* -------- CONTAINER -------- */
        new SupportedExtension(extension: ".3gp", type: "CONTAINER", mp4boxSupport: true),
        new SupportedExtension(".3g2",  "CONTAINER", true),
        new SupportedExtension(".asf",  "CONTAINER", false),
        new SupportedExtension(".avi",  "CONTAINER", true),
        new SupportedExtension(".flv",  "CONTAINER", false),
        new SupportedExtension(".mp4",  "CONTAINER", true),
        new SupportedExtension(".m4v",  "CONTAINER", true),
        new SupportedExtension(".mkv",  "CONTAINER", false),
        new SupportedExtension(".mov",  "CONTAINER", false),
        new SupportedExtension(".m2t",  "CONTAINER", true),
        new SupportedExtension(".m2ts", "CONTAINER", true),
        new SupportedExtension(".mxf",  "CONTAINER", false),
        new SupportedExtension(".ogm",  "CONTAINER", false),
        new SupportedExtension(".rm",   "CONTAINER", false),
        new SupportedExtension(".rmvb", "CONTAINER", false),
        new SupportedExtension(".ts",   "CONTAINER", true),
        new SupportedExtension(".webm", "CONTAINER", false),
        new SupportedExtension(".wmv",  "CONTAINER", false),
        /* ---------- VIDEO ---------- */
        new SupportedExtension(extension: ".263", type: "VIDEO", mp4boxSupport: true),
        new SupportedExtension(".h263", "VIDEO", true),
        new SupportedExtension(".264",  "VIDEO", true),
        new SupportedExtension(".h264", "VIDEO", true),
        new SupportedExtension(".265",  "VIDEO", true),
        new SupportedExtension(".h265", "VIDEO", true),
        new SupportedExtension(".hevc", "VIDEO", true),
        new SupportedExtension(".ivf",  "VIDEO", true),
        new SupportedExtension(".obu",  "VIDEO", true),
        new SupportedExtension(".y4m",  "VIDEO", true),
        /* ---------- AUDIO ---------- */
        new SupportedExtension(extension: ".aa3", type: "AUDIO", mp4boxSupport: false),
        new SupportedExtension(".aac",  "AUDIO", true),
        new SupportedExtension(".aif",  "AUDIO", false),
        new SupportedExtension(".ac3",  "AUDIO", true),
        new SupportedExtension(".ape",  "AUDIO", false),
        new SupportedExtension(".dts",  "AUDIO", false),
        new SupportedExtension(".flac", "AUDIO", true),
        new SupportedExtension(".m1a",  "AUDIO", true),
        new SupportedExtension(".m2a",  "AUDIO", true),
        new SupportedExtension(".mp2",  "AUDIO", true),
        new SupportedExtension(".mp3",  "AUDIO", true),
        new SupportedExtension(".m4a",  "AUDIO", true),
        new SupportedExtension(".oma",  "AUDIO", false),
        new SupportedExtension(".opus", "AUDIO", true),
        new SupportedExtension(".thd",  "AUDIO", false),
        new SupportedExtension(".tta",  "AUDIO", false),
        new SupportedExtension(".wav",  "AUDIO", true),
        new SupportedExtension(".wma",  "AUDIO", false)
    };

    public Extensions()
    {
        SetSupportFor("CONTAINER");
        SetSupportFor("VIDEO");
        SetSupportFor("AUDIO");

        SetFilterFor("CONTAINER");
        SetFilterFor("VIDEO");
        SetFilterFor("AUDIO");
    }

    private void SetSupportFor(string fileType)
    {
        StringBuilder sbSupport = new();

        supportedExtensions
            .Where(ext => ext.Type == fileType)
            .ToList()
            .ForEach(ext => sbSupport.Append($"*{ext.Extension};"));

        string support = sbSupport.ToString()[0..^1];

        switch (fileType)
        {
            case "CONTAINER":
                SupportedContainerExts = support;
                break;
            case "VIDEO":
                SupportedVideoExts = support;
                break;
            case "AUDIO":
                SupportedAudioExts = support;
                break;
        }
    }

    private void SetFilterFor(string fileType)
    {
        StringBuilder sbFilter = new();

        supportedExtensions
            .Where(ext => ext.Type == fileType)
            .ToList()
            .ForEach(ext => sbFilter.Append($"{ext.Extension[1..].ToUpper()} "));

        string filter = sbFilter.ToString()[0..^1];

        switch (fileType)
        {
            case "CONTAINER":
                FilterContainerExts = filter;
                break;
            case "VIDEO":
                FilterVideoExts = filter;
                break;
            case "AUDIO":
                FilterAudioExts = filter;
                break;
        }
    }

    public string DetermineInputFileType(string fileExt) =>
        supportedExtensions.Single(ext => ext.Extension == fileExt).Type;

    public bool IsSupportedByMP4Box(string fileExt) =>
        supportedExtensions.Single(ext => ext.Extension == fileExt).MP4BoxSupport;
}
