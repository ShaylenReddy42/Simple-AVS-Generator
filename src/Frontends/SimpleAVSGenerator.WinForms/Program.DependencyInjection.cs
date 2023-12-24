using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SimpleAVSGeneratorCore;

namespace SimpleAVSGenerator;

internal static partial class Program
{
    private static ServiceProvider ConfigureServices()
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

        services.AddCoreServices();

        return services.BuildServiceProvider();
    }
}
