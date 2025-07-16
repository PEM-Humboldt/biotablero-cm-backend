namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Application services dependencies registry
/// </summary>
public static class ConfigAppServices
{
    /// <summary>
    /// Add custom applications services
    /// </summary>
    /// <param name="services">Application services</param>
    /// <returns>Host builder configuration</returns>
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ILogService, LogService>();

        return services;
    }
}
