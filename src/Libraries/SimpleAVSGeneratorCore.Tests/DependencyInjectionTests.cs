using SimpleAVSGeneratorCore.Services;
using SimpleAVSGeneratorCore.Support;

namespace SimpleAVSGeneratorCore.Tests;

public class DependencyInjectionTests
{
    [Fact(DisplayName = "Validate Registered Core Services")]
    public void ValidateRegisteredServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddCoreServices();

        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();

        var aviSynthScriptService = scope.ServiceProvider.GetService<AviSynthScriptService>();
        var extensions = scope.ServiceProvider.GetService<Extensions>();
        var outputScriptsService = scope.ServiceProvider.GetService<OutputScriptsService>();

        var mediaInfo = scope.ServiceProvider.GetService<MediaInfo.MediaInfo>();

        var fileWriterService = scope.ServiceProvider.GetService<IFileWriterService>();
        var inputFileHandlerService = scope.ServiceProvider.GetService<IInputFileHandlerService>();

        // Assert
        Assert.NotNull(aviSynthScriptService);
        Assert.NotNull(extensions);
        Assert.NotNull(outputScriptsService);

        Assert.NotNull(mediaInfo);

        Assert.NotNull(fileWriterService);
        Assert.NotNull(inputFileHandlerService);

        Assert.IsType<FileWriterService>(fileWriterService);
        Assert.IsType<InputFileHandlerService>(inputFileHandlerService);
    }
}
