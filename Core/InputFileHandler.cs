namespace Simple_AVS_Generator.Core
{
    internal class InputFileHandler
    {
        public Common? Common = null;
        public InputFileHandler(string fileName)
        {
            Common = new(fileName);
        }

        private void WriteFile(string outputFileName, string fileContents)
        {
            StreamWriter sw = new StreamWriter(outputFileName);
            sw.Write($"{(outputFileName.EndsWith(".cmd") ? "@ECHO off\r\n\r\n" : "")}{fileContents}");
            sw.Close();
        }

        public void CreateScripts()
        {
            AviSynthScript script = new(Common);
            
            script.SetScriptContent();
            WriteFile(script.ScriptFile, script.ScriptContent);

            WriteFile(Common.AVSMeterScriptFile, Common.AVSMeterScriptContents);

            OutputScripts output = new(Common);

            output.ConfigureVideoScript();
            if (output.VideoEncoderScriptContents is not null)
            {
                WriteFile(output.VideoEncoderScriptFile, output.VideoEncoderScriptContents);
            }

            output.ConfigureAudioScript();
            if (output.AudioEncoderScriptContents is not null)
            {
                WriteFile(output.AudioEncoderScriptFile, output.AudioEncoderScriptContents);
            }

            output.ConfigureContainerScript();
            if (output.ContainerScriptContents is not null)
            {
                WriteFile(output.ContainerScriptFile, output.ContainerScriptContents);
            }
        }
    }
}
