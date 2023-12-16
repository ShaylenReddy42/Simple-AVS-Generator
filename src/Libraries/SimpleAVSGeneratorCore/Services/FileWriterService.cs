namespace SimpleAVSGeneratorCore.Services;

public class FileWriterService : IFileWriterService
{
    public async Task WriteFileAsync(string outputFileName, string fileContents)
    {
        var solutionConfiguration = string.Empty;

#if DEBUG
        solutionConfiguration = "DEBUG";
#else
        solutionConfiguration = "RELEASE";
#endif

        if (solutionConfiguration is "DEBUG")
        {
            return;
        }

        await using var streamWriter = new StreamWriter(outputFileName);
        await streamWriter.WriteAsync($"{(outputFileName.EndsWith(".cmd") ? "@ECHO off\r\n\r\n" : "")}{fileContents}");
    }
}
