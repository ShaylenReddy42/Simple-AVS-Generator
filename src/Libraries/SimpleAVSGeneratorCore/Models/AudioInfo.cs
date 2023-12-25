using static SimpleAVSGeneratorCore.Support.Audio;

namespace SimpleAVSGeneratorCore.Models;

public class AudioInfo
{
    public bool Enabled { get; set; } = default;
    public string? SourceChannels { get; init; }
    public string Codec { get; set; } = string.Empty;
    public int Bitrate { get; set; }
    public string Language { get; set; } = "Undetermined";
    public string LanguageCode => immutableLanguagesDictionary[Language];
    public string Extension => 
        Codec is not "" ? immutableSupportedOutputAudios.First(audio => audio.Codec == Codec).Extension : string.Empty;
}