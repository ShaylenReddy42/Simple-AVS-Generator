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

namespace SimpleAVSGeneratorCore.Support;

public class Audio
{
    public static Dictionary<string, string> outputAudioCodecsDictionary = new()
    {
        { "AAC-LC", ".m4a" },
        { "AAC-HE", ".m4a" },
        { "OPUS",   ".ogg" }
    };

    public static Dictionary<string, string> languagesDictionary = new()
    {
        { "English",      "eng" },
        { "Hindi",        "hin" },
        { "Japanese",     "jpn" },
        { "Tamil",        "tam" },
        { "Undetermined", "und" }
    };

    private static string[] outputAudioChannels =
    {
        "Stereo",
        "Surround 5.1",
        "Surround 7.1"
    };

    private static Dictionary<string, Dictionary<string, object[]>> selectableAudioBitratesDictionary = new()
    {
        { 
            "AAC-LC",  
            new()
            {
                { "Stereo",       new object[] {  96, 112, 128, 144, 160, 192 } },
                { "Surround 5.1", new object[] { 192, 224, 256, 288, 320, 384 } },
                { "Surround 7.1", new object[] { 384, 448, 512, 576, 640, 768 } }
            }
        },
        {
            "AAC-HE",
            new()
            {
                { "Stereo",       new object[] {  32,  40,  48,  56,  64,  80 } },
                { "Surround 5.1", new object[] {  80,  96, 112, 128, 160, 192 } },
                { "Surround 7.1", new object[] { 112, 128, 160, 192, 224, 256 } }
            }
        },
        {
            "OPUS",
            new()
            {
                { "Stereo",       new object[] {  96, 112, 128, 144, 160, 192 } },
                { "Surround 5.1", new object[] { 144, 160, 192, 224, 256, 288 } },
                { "Surround 7.1", new object[] { 256, 288, 320, 384, 448, 576 } }
            }
        }
    };

    private static Dictionary<string, Dictionary<string, int>> defaultAudioBitratesDictionary = new()
    {
        {
            "AAC-LC",
            new()
            {
                { "Stereo",       128 },
                { "Surround 5.1", 384 },
                { "Surround 7.1", 512 }
            }
        },
        {
            "AAC-HE",
            new()
            {
                { "Stereo",        80 },
                { "Surround 5.1", 192 },
                { "Surround 7.1", 256 }
            }
        },
        {
            "OPUS",
            new()
            {
                { "Stereo",        96 },
                { "Surround 5.1", 288 },
                { "Surround 7.1", 384 }
            }
        }
    };

    public static object[] GetOutputAudioCodecs()
    {
        return outputAudioCodecsDictionary.Keys.ToArray();
    }

    public static object[] GetLanguages()
    {
        return languagesDictionary.Keys.ToArray();
    }

    public static object[] GetAudioChannels()
    {
        return outputAudioChannels;
    }

    public static (object[], int) GetSelectableAndDefaultAudioBitrates(string audioCodec, string audioChannels)
    {
        Dictionary<string, object[]> audioCodecBitratesDictionary = selectableAudioBitratesDictionary[audioCodec];
        Dictionary<string, int> audioCodecDefaultAudioBitratesDictionary = defaultAudioBitratesDictionary[audioCodec];

        return (audioCodecBitratesDictionary[audioChannels], audioCodecDefaultAudioBitratesDictionary[audioChannels]);
    }
}
