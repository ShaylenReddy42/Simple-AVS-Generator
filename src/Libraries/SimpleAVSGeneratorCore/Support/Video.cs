using System.Collections.Immutable;

namespace SimpleAVSGeneratorCore.Support;

public static class Video
{
    private static readonly Dictionary<string, string> outputVideoCodecsDictionary = new()
    {
        { "HEVC",         ".265" },
        { "AV1",          ".ivf" },
        { "AVC",          ".264" },
        { "WhatsApp",     ".264" },
        { "Mux Original", ""     }
    };

    public static readonly ImmutableDictionary<string, string> idOutputVideoCodecsDictionary = outputVideoCodecsDictionary.ToImmutableDictionary();

    private static readonly Dictionary<string, int> keyframeIntervalDictionary = new()
    {
        { "2 Seconds",   2 },
        { "5 Seconds",   5 },
        { "10 Seconds", 10 }
    };

    public static readonly ImmutableDictionary<string, int> idKeyframeIntervalDictionary = keyframeIntervalDictionary.ToImmutableDictionary();

    public static Task<object[]> GetOutputVideoCodecsAsync() => Task.FromResult<object[]>(outputVideoCodecsDictionary.Keys.ToArray());

    public static Task<object[]> GetKeyframeIntervalsAsync() => Task.FromResult<object[]>(keyframeIntervalDictionary.Keys.ToArray());
}
