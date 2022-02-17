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

namespace SimpleAVSGenerator.Core.Support;

public class Audio
{
    public static string [,] languages =
    {
        //ISO 639-2 language code | Name of language in English
        { "eng", "English"      },
        { "hin", "Hindi"        },
        { "jpn", "Japanese"     },
        { "tam", "Tamil"        },
        { "und", "Undetermined" }
    };

    public static string [,] outputAudioCodecs =
    {
        { ".m4a", "AAC-LC" },
        { ".m4a", "AAC-HE" },
        { ".ogg", "OPUS"   }
    };

    public static string [] outputAudioChannels =
    {
        "Stereo",
        "Surround 5.1",
        "Surround 7.1"
    };

    /**
     * A 3D array containing sane audio bitrates
     * for each codec and their channel layouts
     * 
     * 1st Dimension: Codec [AAC-LC, AAC-HE, OPUS]
     * 2nd Dimension: Channels [2, 5.1, 7.1]
     * 3rd Dimension: List of sane bitrates
     */
    public static int [,,] selectableAudioBitrates =
    {
        //AAC-LC
        {
            {  96, 112, 128, 144, 160, 192 }, //2 Channels
            { 192, 224, 256, 288, 320, 384 }, //5.1 Channels
            { 384, 448, 512, 576, 640, 768 }  //7.1 Channels
        },

        //AAC-HE
        {
            {  32,  40,  48,  56,  64,  80 }, //2 Channels
            {  80,  96, 112, 128, 160, 192 }, //5.1 Channels
            { 112, 128, 160, 192, 224, 256 }  //7.1 Channels
        },

        //OPUS
        {
            {  96, 112, 128, 144, 160, 192 }, //2 Channels
            { 144, 160, 192, 224, 256, 288 }, //5.1 Channels
            { 256, 288, 320, 384, 448, 576 }  //7.1 Channels
        }
    };

    /**
     * 1st Dimension: Codec [AAC-LC, AAC-HE, OPUS]
     * 2nd Dimension: Channels [2, 5.1, 7.1]
     */
    public static int [,] defaultAudioBitrates =
    {
        { 128, 384, 512 }, //AAC-LC
        {  80, 192, 256 }, //AAC-HE
        {  96, 288, 384 }  //OPUS
    };
}
