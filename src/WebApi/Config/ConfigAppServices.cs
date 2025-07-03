namespace IAVH.BioTablero.CM.WebApi.Config;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services;

using Microsoft.Extensions.DependencyInjection;

public static class ConfigAppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ILogService, LogService>();

        return services;
    }
}
