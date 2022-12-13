namespace SimpleAVSGeneratorCore.Models;

public class SupportedOutputAudio
{
    public string Codec { get; }
    public string Channels { get; }
    public object[] Bitrates { get; }
    public int DefaultBitrate { get; }
    public string Extension => Codec switch
    {
        "AAC-LC" => ".m4a",
        "AAC-HE" => ".m4a",
        "OPUS"   => ".ogg",
        _        => string.Empty
    };

    public SupportedOutputAudio(string codec, string channels, object[] bitrates, int defaultBitrate) => 
        (Codec, Channels, Bitrates, DefaultBitrate) = (codec, channels, bitrates, defaultBitrate);
}
