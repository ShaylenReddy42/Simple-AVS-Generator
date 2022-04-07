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

using static SimpleAVSGeneratorCore.Support.Audio;

namespace SimpleAVSGeneratorCore.Models;

public class AudioModel
{
    public bool Enabled { get; set; } = default;
    public string? SourceChannels { get; init; }
    public string Codec { get; set; } = string.Empty;
    public int Bitrate { get; set; }
    public string Language { get; set; } = "Undetermined";
    public string LanguageCode => languagesDictionary[Language];
    public string Extension => Codec is not "" ? outputAudioCodecsDictionary[Codec] : string.Empty;
}