namespace SimpleAVSGeneratorCore.Models;

public class FileModel
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string FileName { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string FileExt => Path.GetExtension(FileName).ToLower();
    public string FileNameOnly => Path.GetFileNameWithoutExtension(FileName);
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string FileType { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public bool IsSupportedByMP4Box { get; set; } = default;

    public bool HasVideo { get; init; }
    public bool HasAudio { get; init; }
}