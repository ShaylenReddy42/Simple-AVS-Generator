namespace Simple_AVS_Generator.Modules
{
    internal class Common
    {
        public static void WriteFile(string outputFileName, string fileContents)
        {
            StreamWriter sw = new StreamWriter(outputFileName);
            sw.Write($"{(outputFileName.EndsWith(".cmd") ? "@ECHO off\r\n\r\n" : "")}{fileContents}");
            sw.Close();
        }
    }
}
