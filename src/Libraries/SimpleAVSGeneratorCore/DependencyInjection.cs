using Microsoft.Extensions.DependencyInjection;
using SimpleAVSGeneratorCore.Services;
using SimpleAVSGeneratorCore.Support;

namespace SimpleAVSGeneratorCore;

public static class DependencyInjection
{
    /// <summary>
    /// Plugs in the services created in the core library to any service collection
    /// </summary>
    /// <param name="services">The original service collection</param>
    /// <returns>The original service collection with the addition of core services</returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<AviSynthScriptService>();
        services.AddSingleton<Extensions>();
        services.AddScoped<OutputScriptsService>();

        services.AddScoped<MediaInfo.MediaInfo>();

        services.AddSingleton<IFileWriterService, FileWriterService>();
        services.AddSingleton<IInputFileHandlerService, InputFileHandlerService>();

        return services;
    }
}
