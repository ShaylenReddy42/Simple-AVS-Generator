using System.Collections.Immutable;

namespace SimpleAVSGeneratorCore.Support;

public static class Video
{
    private static Dictionary<string, string> outputVideoCodecsDictionary = new()
    {
        { "HEVC",         ".265" },
        { "AV1",          ".ivf" },
        { "AVC",          ".264" },
        { "WhatsApp",     ".264" },
        { "Mux Original", ""     }
    };

    public static readonly ImmutableDictionary<string, string> idOutputVideoCodecsDictionary = outputVideoCodecsDictionary.ToImmutableDictionary();

    private static Dictionary<string, int> keyframeIntervalDictionary = new()
    {
        { "2 Seconds",   2 },
        { "5 Seconds",   5 },
        { "10 Seconds", 10 }
    };

    public static readonly ImmutableDictionary<string, int> idKeyframeIntervalDictionary = keyframeIntervalDictionary.ToImmutableDictionary();

    public static object[] GetOutputVideoCodecs() => outputVideoCodecsDictionary.Keys.ToArray();

    public static object[] GetKeyframeIntervals() => keyframeIntervalDictionary.Keys.ToArray();
}
