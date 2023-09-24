using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SimpleAVSGeneratorCore.Support;

namespace SimpleAVSGenerator;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        var services = ConfigureServices();

        Application.Run(services.GetRequiredService<MainForm>());
    }

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

        services.AddSingleton<Extensions>();

        return services.BuildServiceProvider();
    }
}
