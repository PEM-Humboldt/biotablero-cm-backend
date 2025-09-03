namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Iam;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// External services dependencies registry.
/// </summary>
public static class ConfigExternalServices
{
    /// <summary>
    /// Add custom external services.
    /// </summary>
    /// <param name="services">Application services.</param>
    /// <returns>Host builder configuration.</returns>
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        // Custom repositories
        services.AddScoped<IInitiativeRepository, InitiativeRepository>();

        // External services
        services.AddScoped<IStorageService, StorageService>();
        services.AddSingleton<IIamService, IamService>();

        return services;
    }
}
