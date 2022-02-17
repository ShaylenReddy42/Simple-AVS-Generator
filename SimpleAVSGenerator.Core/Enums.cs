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

namespace SimpleAVSGenerator.Core;

public class Enums
{
    public enum ExtensionTypes
    {
        CONTAINER = 0,
        VIDEO = 1,
        AUDIO = 2
    }

    public enum VideoCodecs
    {
        HEVC = 0,
        AV1 = 1,
        AVC = 2,
        WhatsApp = 3,
        Original = 4
    }

    public enum AudioCodecs
    {
        AAC_LC = 0,
        AAC_HE = 1,
        OPUS = 2
    }

    public enum OutputContainers
    {
        MP4 = 0,
        MKV = 1
    }
}
