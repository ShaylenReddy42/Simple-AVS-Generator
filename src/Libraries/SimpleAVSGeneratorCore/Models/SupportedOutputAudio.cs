using SimpleAVSGeneratorCore.Constants;

namespace SimpleAVSGeneratorCore.Models;

public class SupportedOutputAudio
{
    public string Codec { get; }
    public string Channels { get; }
    public object[] Bitrates { get; }
    public int DefaultBitrate { get; }
    public string Extension => Codec switch
    {
        SupportedOutputAudioCodecs.AacLc => ".m4a",
        SupportedOutputAudioCodecs.AacHe => ".m4a",
        SupportedOutputAudioCodecs.Opus  => ".ogg",
        _                                => string.Empty
    };

    public SupportedOutputAudio(string codec, string channels, object[] bitrates, int defaultBitrate) => 
        (Codec, Channels, Bitrates, DefaultBitrate) = (codec, channels, bitrates, defaultBitrate);
}
