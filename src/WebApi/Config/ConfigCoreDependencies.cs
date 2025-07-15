namespace IAVH.BioTablero.CM.WebApi.Config;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Services;
using IAVH.BioTablero.CM.Persistence.Repositories;
using IAVH.BioTablero.CM.WebApi.Controllers.Tools;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Core dependencies registry
/// </summary>
public static class ConfigCoreDependencies
{
    /// <summary>
    /// Add core services
    /// </summary>
    /// <param name="services">Service descriptors collection</param>
    /// <returns>Service descriptors collection with custom services</returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddHttpContextAccessor(); // Required for Serilog (ASP.NET)

        // Ardalis repository registry
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<IWebTools, WebTools>();

        // Enum services setup
        services.AddSingleton(typeof(IServiceReadEnum<>), typeof(ServiceReadEnum<>));

        return services;
    }
}
