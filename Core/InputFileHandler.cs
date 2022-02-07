﻿namespace Simple_AVS_Generator.Core
{
    internal class InputFileHandler
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
