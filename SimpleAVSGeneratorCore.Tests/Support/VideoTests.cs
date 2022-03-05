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

using Xunit;

using static SimpleAVSGeneratorCore.Support.Video;

namespace SimpleAVSGeneratorCore.Tests.Support;

public class VideoTests
{
    [Fact(DisplayName = "Validate Video Codecs")]
    public void GetOutputVideoCodecs_ValidateVideoCodecs()
    {
        // Arrange
        object[] expectedVideoCodecs = new object[] { "HEVC", "AV1", "AVC", "WhatsApp", "Mux Original" };

        // Act
        object[] actualVideoCodecs = GetOutputVideoCodecs();

        // Assert
        Assert.Equal(expectedVideoCodecs, actualVideoCodecs);
    }

    [Fact(DisplayName = "Validate Source FPS")]
    public void GetSourceFPS_ValidateSourceFPS()
    {
        // Arrange
        object[] expectedSourceFPS = new object[] { "23.976 / 24", "25", "29.97 / 30", "59.94 / 60" };

        // Act
        object[] actualSourceFPS = GetSourceFPS();

        // Assert
        Assert.Equal(expectedSourceFPS, actualSourceFPS);
    }

    [Fact(DisplayName = "Validate Keyframe Intervals In Seconds")]
    public void GetKeyframeIntervals_ValidateKeyframeIntervalsInSeconds()
    {
        // Arrange
        object[] expectedKeyframeIntervals = new object[] { "2 Seconds", "5 Seconds", "10 Seconds" };

        // Act
        object[] actualKeyframeIntervals = GetKeyframeIntervals();

        // Assert
        Assert.Equal(expectedKeyframeIntervals, actualKeyframeIntervals);
    }
}
