namespace IAVH.BioTablero.CM.WebApi.Config;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Mappings;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using IAVH.BioTablero.CM.Core.Entities.LogNS;


using Microsoft.Extensions.DependencyInjection;

public static class ConfigMappings
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddScoped<IMapper<LogEntity, LogDto>, LogMappings>();

        return services;
    }
}
