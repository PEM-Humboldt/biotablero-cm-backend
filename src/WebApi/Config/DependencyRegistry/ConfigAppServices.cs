namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.Geo;
using IAVH.BioTablero.CM.Application.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Services.Logging;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Application services dependencies registry.
/// </summary>
public static class ConfigAppServices
{
    /// <summary>
    /// Add custom applications services.
    /// </summary>
    /// <param name="services">Application services.</param>
    /// <returns>Host builder configuration.</returns>
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IInitiativeContactService, InitiativeContactService>();
        services.AddScoped<IInitiativeLocationService, InitiativeLocationService>();
        services.AddScoped<IInitiativeService, InitiativeService>();
        services.AddScoped<IInitiativeUserService, InitiativeUserService>();
        services.AddScoped<IInitiativeTagService, InitiativeTagService>();
        services.AddScoped<IInitiativeTagInitiativeService, InitiativeTagInitiativeService>();

        return services;
    }
}
