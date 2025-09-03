namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using IAVH.BioTablero.CM.WebApi.Services;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Scheduled tasks configuration.
/// </summary>
public static class ConfigTasks
{
    /// <summary>
    /// Add scheduled tasks.
    /// </summary>
    /// <param name="services">Application services.</param>
    /// <returns>Host builder configuration.</returns>
    public static IServiceCollection AddTasks(this IServiceCollection services)
    {
        services.AddHostedService<JoinRequestTasks>();

        return services;
    }
}
