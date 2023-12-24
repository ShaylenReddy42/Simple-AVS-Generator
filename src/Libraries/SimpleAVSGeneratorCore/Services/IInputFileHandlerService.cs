using SimpleAVSGeneratorCore.Models;

namespace SimpleAVSGeneratorCore.Services;

public interface IInputFileHandlerService
{
    Task<InputFile> CreateInputFileAsync(string fileName, string home);
    Task<string> CreateScriptsAsync(InputFile inputFile);
}
