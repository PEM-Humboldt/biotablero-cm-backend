namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Mappings;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using IAVH.BioTablero.CM.Core.Entities.LogNS;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Mappings configuration
/// </summary>
public static class ConfigMappings
{
    /// <summary>
    /// Add system mappings
    /// </summary>
    /// <param name="services">Application services</param>
    /// <returns>Host builder configuration</returns>
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddScoped<IMapper<LogEntity, LogDto>, LogMappings>();

        return services;
    }
}
