using SimpleAVSGeneratorCore.Models;
using System.Collections.Immutable;

namespace SimpleAVSGeneratorCore.Support;

public static class Audio
{
    private static readonly Dictionary<string, string> languagesDictionary = new()
    {
        { "English",      "eng" },
        { "Hindi",        "hin" },
        { "Japanese",     "jpn" },
        { "Tamil",        "tam" },
        { "Undetermined", "und" }
    };

    public static readonly ImmutableDictionary<string, string> immutableLanguagesDictionary = languagesDictionary.ToImmutableDictionary();

    private static readonly List<SupportedOutputAudio> supportedOutputAudios =
    [
        new SupportedOutputAudio(codec: "AAC-LC", channels: "1.0", bitrates: [ 48,  56,  64,  72,  80,  96 ], defaultBitrate: 64),
        new SupportedOutputAudio("AAC-LC", "2.0", [  96, 112, 128, 144, 160, 192 ], 128),
        new SupportedOutputAudio("AAC-LC", "5.1", [ 192, 224, 256, 288, 320, 384 ], 384),
        new SupportedOutputAudio("AAC-LC", "7.1", [ 384, 448, 512, 576, 640, 768 ], 512),

        new SupportedOutputAudio(codec: "AAC-HE", channels: "1.0", bitrates: [ 16,  20,  24,  28,  32,  40 ], defaultBitrate: 40),
        new SupportedOutputAudio("AAC-HE", "2.0", [  32,  40,  48,  56,  64,  80 ],  80),
        new SupportedOutputAudio("AAC-HE", "5.1", [  80,  96, 112, 128, 160, 192 ], 192),
        new SupportedOutputAudio("AAC-HE", "7.1", [ 112, 128, 160, 192, 224, 256 ], 256),

        new SupportedOutputAudio(codec:   "OPUS", channels: "1.0", bitrates: [ 36,  40,  48,  56,  64,  72 ], defaultBitrate: 40),
        new SupportedOutputAudio("OPUS",   "2.0", [  72,  80,  96, 112, 128, 144 ],  80),
        new SupportedOutputAudio("OPUS",   "5.1", [ 160, 192, 224, 240, 256, 288 ], 240),
        new SupportedOutputAudio("OPUS",   "7.1", [ 256, 288, 320, 384, 448, 512 ], 320)
    ];

    public static readonly ImmutableList<SupportedOutputAudio> immutableSupportedOutputAudios = [.. supportedOutputAudios];

    public static Task<object[]> GetOutputAudioCodecsAsync() =>
        Task.FromResult<object[]>(supportedOutputAudios.Select(audio => audio.Codec).Distinct().ToArray());

    public static Task<object[]> GetLanguagesAsync() =>
        Task.FromResult<object[]>(languagesDictionary.Keys.ToArray());

    public static Task<(object[], int)> GetSelectableAndDefaultAudioBitratesAsync(string audioCodec, string audioChannels)
    {
        var supportedOutputAudio =
            supportedOutputAudios
                .Single(audio => audio.Codec == audioCodec && audio.Channels == audioChannels);

        return Task.FromResult((supportedOutputAudio.Bitrates, supportedOutputAudio.DefaultBitrate));
    }
}
