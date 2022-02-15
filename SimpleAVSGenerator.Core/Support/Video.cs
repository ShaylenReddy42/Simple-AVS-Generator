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

namespace SimpleAVSGenerator.Core.Support
{
    public class Video
    {
        public static object[,] sourceFPS =
        {
            { 24, "23.976 / 24" },
            { 25, "25"          },
            { 30, "29.97 / 30"  },
            { 60, "59.94"       }
        };

        public static object[,] keyframeInterval =
        {
            {  2, "2 Seconds"  },
            {  5, "5 Seconds"  },
            { 10, "10 Seconds" }
        };

        public static string[,] outputVideoCodecs =
        {
            { ".265", "HEVC"         },
            { ".ivf", "AV1"          },
            { ".264", "AVC"          },
            { ".264", "WhatsApp"     },
            {     "", "Mux Original" }
        };
    }
}
