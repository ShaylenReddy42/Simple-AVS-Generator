using SimpleAVSGeneratorCore.Services;

namespace SimpleAVSGeneratorCore.Tests.Fixtures;

public class CommonDependencyInjectionFixture : IDisposable
{
    public IInputFileHandlerService InputFileHandlerServiceInstance { get; }

    public CommonDependencyInjectionFixture()
    {
        var fileWriterService = new FileWriterService();

        var serviceProvider =
            new ServiceCollection()
                .AddScoped<MediaInfo.MediaInfo>()
        .BuildServiceProvider();

        InputFileHandlerServiceInstance = new InputFileHandlerService(fileWriterService, serviceProvider);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // Cleanup
    }
}
