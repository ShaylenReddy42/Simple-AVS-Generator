using static SimpleAVSGeneratorCore.Support.Audio;

namespace SimpleAVSGeneratorCore.Models;

public class AudioModel
{
    public bool Enabled { get; set; } = default;
    public string? SourceChannels { get; init; }
    public string Codec { get; set; } = string.Empty;
    public int Bitrate { get; set; }
    public string Language { get; set; } = "Undetermined";
    public string LanguageCode => idLanguagesDictionary[Language];
    public string Extension => 
        Codec is not "" ? supportedOutputAudios.First(audio => audio.Codec == Codec).Extension : string.Empty;
}