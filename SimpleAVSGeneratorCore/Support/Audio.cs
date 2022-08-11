/******************************************************************************
 * Copyright (C) 2022 Shaylen Reddy
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along
 * with this program; if not, write to the Free Software Foundation, Inc.,
 * 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
 ******************************************************************************/

using System.Collections.Immutable;

namespace SimpleAVSGeneratorCore.Support;

public static class Audio
{
    private static Dictionary<string, string> outputAudioCodecsDictionary = new()
    {
        { "AAC-LC", ".m4a" },
        { "AAC-HE", ".m4a" },
        { "OPUS",   ".ogg" }
    };

    public static readonly ImmutableDictionary<string, string> idOutputAudioCodecsDictionary = outputAudioCodecsDictionary.ToImmutableDictionary();

    private static Dictionary<string, string> languagesDictionary = new()
    {
        { "English",      "eng" },
        { "Hindi",        "hin" },
        { "Japanese",     "jpn" },
        { "Tamil",        "tam" },
        { "Undetermined", "und" }
    };

    public static readonly ImmutableDictionary<string, string> idLanguagesDictionary = languagesDictionary.ToImmutableDictionary();

    private static Dictionary<string, Dictionary<string, object[]>> selectableAudioBitratesDictionary = new()
    {
        { 
            "AAC-LC",  
            new()
            {
                { "1.0", new object[] {  48,  56,  64,  72,  80,  96 } },
                { "2.0", new object[] {  96, 112, 128, 144, 160, 192 } },
                { "5.1", new object[] { 192, 224, 256, 288, 320, 384 } },
                { "7.1", new object[] { 384, 448, 512, 576, 640, 768 } }
            }
        },
        {
            "AAC-HE",
            new()
            {
                { "1.0", new object[] {  16,  20,  24,  28,  32,  40 } },
                { "2.0", new object[] {  32,  40,  48,  56,  64,  80 } },
                { "5.1", new object[] {  80,  96, 112, 128, 160, 192 } },
                { "7.1", new object[] { 112, 128, 160, 192, 224, 256 } }
            }
        },
        {
            "OPUS",
            new()
            {
                { "1.0", new object[] {  36,  40,  48,  56,  64,  72 } },
                { "2.0", new object[] {  72,  80,  96, 112, 128, 144 } },
                { "5.1", new object[] { 160, 192, 224, 240, 256, 288 } },
                { "7.1", new object[] { 256, 288, 320, 384, 448, 512 } }
            }
        }
    };

    private static Dictionary<string, Dictionary<string, int>> defaultAudioBitratesDictionary = new()
    {
        {
            "AAC-LC",
            new()
            {
                { "1.0",  64 },
                { "2.0", 128 },
                { "5.1", 384 },
                { "7.1", 512 }
            }
        },
        {
            "AAC-HE",
            new()
            {
                { "1.0",  40 },
                { "2.0",  80 },
                { "5.1", 192 },
                { "7.1", 256 }
            }
        },
        {
            "OPUS",
            new()
            {
                { "1.0",  40 },
                { "2.0",  80 },
                { "5.1", 240 },
                { "7.1", 320 }
            }
        }
    };

    public static object[] GetOutputAudioCodecs() => outputAudioCodecsDictionary.Keys.ToArray();

    public static object[] GetLanguages() => languagesDictionary.Keys.ToArray();

    public static (object[], int) GetSelectableAndDefaultAudioBitrates(string audioCodec, string audioChannels)
    {
        Dictionary<string, object[]> audioCodecBitratesDictionary = selectableAudioBitratesDictionary[audioCodec];
        Dictionary<string, int> audioCodecDefaultAudioBitratesDictionary = defaultAudioBitratesDictionary[audioCodec];

        return (audioCodecBitratesDictionary[audioChannels], audioCodecDefaultAudioBitratesDictionary[audioChannels]);
    }
}
