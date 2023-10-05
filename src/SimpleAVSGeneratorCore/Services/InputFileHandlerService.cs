namespace SimpleAVSGeneratorCore.Services;

public class InputFileHandlerService : IInputFileHandlerService
{
    private readonly IFileWriterService fileWriterService;

    public InputFileHandlerService(IFileWriterService fileWriterService)
    {
        this.fileWriterService = fileWriterService;
    }

    public async Task<string> CreateScriptsAsync(InputFile inputFile)
    {
        // scriptsCreated is a variable that will be used for testing this function
        // Result could be in variable length, containing characters to indicated
        // which scripts were created e.g. svac
        // Key:
        // s indicates that the AviSynth script is created
        // v indicates that the Video Encoder script is created
        // a indicates that the Audio Encoder script is created
        // c indicates that the Container Muxing script is created

        var scriptsCreated = string.Empty;

#if RELEASE
        Directory.CreateDirectory(inputFile.OutputDir);
#endif

        AviSynthScript script = new(inputFile.ScriptFile);

        await script.SetScriptContentAsync(inputFile.FileInfo, inputFile.Video, inputFile.Audio);
        if (script.CreateAviSynthScript)
        {
            scriptsCreated += "s";

            await fileWriterService.WriteFileAsync(script.AVSScriptFile, script.AVSScriptContent);

            await fileWriterService.WriteFileAsync(inputFile.AVSMeterScriptFile, InputFile.AVSMeterScriptContent);
        }

        OutputScripts output = new();

        await output.ConfigureVideoScriptAsync(inputFile.Video, inputFile.OutputDir);
        if (output.VideoEncoderScriptFile is not null && output.VideoEncoderScriptContent is not null)
        {
            scriptsCreated += "v";

            await fileWriterService.WriteFileAsync(output.VideoEncoderScriptFile, output.VideoEncoderScriptContent);
        }

        await output.ConfigureAudioScriptAsync(inputFile.FileInfo, inputFile.Audio, inputFile.OutputDir);
        if (output.AudioEncoderScriptFile is not null && output.AudioEncoderScriptContent is not null)
        {
            scriptsCreated += "a";

            await fileWriterService.WriteFileAsync(output.AudioEncoderScriptFile, output.AudioEncoderScriptContent);

        }

        await output.ConfigureContainerScriptAsync(inputFile.FileInfo, inputFile.Video, inputFile.Audio, inputFile.OutputContainer, inputFile.OutputDir);
        if (output.ContainerScriptFile is not null && output.ContainerScriptContent is not null)
        {
            scriptsCreated += "c";

            await fileWriterService.WriteFileAsync(output.ContainerScriptFile, output.ContainerScriptContent);
        }

#if RELEASE
        if (scriptsCreated is "")
        {
            Directory.Delete(inputFile.OutputDir);
        }
#endif

        return scriptsCreated;
    }
}
