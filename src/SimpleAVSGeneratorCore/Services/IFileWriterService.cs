namespace SimpleAVSGeneratorCore.Services;

public interface IFileWriterService
{
    Task WriteFileAsync(string outputFileName, string fileContents);
}
