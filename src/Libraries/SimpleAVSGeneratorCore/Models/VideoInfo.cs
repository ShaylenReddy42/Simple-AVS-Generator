using static SimpleAVSGeneratorCore.Support.Video;

namespace SimpleAVSGeneratorCore.Models;

public class VideoInfo
{
    public bool Enabled { get; set; } = default;
    public string Codec { get; set; } = string.Empty;
    public bool MuxOriginalVideo => Codec is "Mux Original";
    public decimal SourceFPS { get; init; }
    public int RoundedFPS => (int)Math.Round(SourceFPS);
    public int SourceFrameCount { get; init; }
    public string KeyframeIntervalInSeconds { get; set; } = "2 Seconds";
    public int KeyframeIntervalInFrames => RoundedFPS * idKeyframeIntervalDictionary[KeyframeIntervalInSeconds];
    public bool NeedsToBeResized => Codec is "WhatsApp";
    public string Extension => Codec is not "" ? idOutputVideoCodecsDictionary[Codec] : string.Empty;
}