namespace SimpleAVSGeneratorCore.Services;

public interface IInputFileHandlerService
{
    Task<string> CreateScriptsAsync(InputFile inputFile);
}
