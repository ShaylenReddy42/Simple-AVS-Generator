using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SimpleAVSGeneratorCore.Services;
using SimpleAVSGeneratorCore.Support;

namespace SimpleAVSGenerator;

internal static partial class Program
{
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(services =>
        {
            return new ConfigurationBuilder()
                .SetBasePath(Application.StartupPath)
                .AddJsonFile("appsettings.json")
                .Build();
        });

        services.AddLogging(configure =>
        {
            var loggerConfiguration =
                new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .WriteTo.Async(configure =>
                        configure.File(
                            path: Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Simple AVS Generator", "simpleavsgenerator-logs-.txt"),
                            outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level}] <{SourceContext}> {NewLine}{Message:lj} {NewLine}{Exception}{NewLine}",
                            rollingInterval: RollingInterval.Day,
                            retainedFileTimeLimit: TimeSpan.FromDays(14)))
                    .CreateLogger();

            configure.AddSerilog(loggerConfiguration);
        });

        services.AddSingleton<MainForm>();

        services.AddScoped<AviSynthScriptService>();
        services.AddSingleton<Extensions>();
        services.AddScoped<OutputScriptsService>();

        services.AddScoped<MediaInfo.MediaInfo>();

        services.AddSingleton<IFileWriterService, FileWriterService>();

        services.AddSingleton<IInputFileHandlerService, InputFileHandlerService>();

        return services.BuildServiceProvider();
    }
}
