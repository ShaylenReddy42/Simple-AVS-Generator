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

namespace Simple_AVS_Generator.Core
{
    public class InputFileHandler
    {
        public Common common;
        
        public InputFileHandler(string fileName)
        {
            common = new(fileName);
        }

        private void WriteFile(string outputFileName, string fileContents)
        {
            StreamWriter sw = new StreamWriter(outputFileName);
            sw.Write($"{(outputFileName.EndsWith(".cmd") ? "@ECHO off\r\n\r\n" : "")}{fileContents}");
            sw.Close();
        }

        public void CreateScripts()
        {
            AviSynthScript script = new(common);
            
            script.SetScriptContent();
            WriteFile(script.AVSScriptFile, script.AVSScriptContent);

            WriteFile(common.AVSMeterScriptFile, common.AVSMeterScriptContent);

            OutputScripts output = new(common);

            output.ConfigureVideoScript();
            if (output.VideoEncoderScriptFile is not null && output.VideoEncoderScriptContent is not null)
            {
                WriteFile(output.VideoEncoderScriptFile, output.VideoEncoderScriptContent);
            }

            output.ConfigureAudioScript();
            if (output.AudioEncoderScriptFile is not null && output.AudioEncoderScriptContent is not null)
            {
                WriteFile(output.AudioEncoderScriptFile, output.AudioEncoderScriptContent);
            }

            output.ConfigureContainerScript();
            if (output.ContainerScriptFile is not null && output.ContainerScriptContent is not null)
            {
                WriteFile(output.ContainerScriptFile, output.ContainerScriptContent);
            }
        }
    }
}
