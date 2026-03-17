namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.DependencyRegistry;

using IAVH.BioTablero.CM.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependencies class for main DB context.
/// </summary>
public static class ConfigDbDependencies
{
    /// <summary>
    /// Add database services.
    /// </summary>
    /// <param name="services">Application services.</param>
    /// <param name="connectionString">Database connection string.</param>
    public static void AddDbServices(IServiceCollection services, string connectionString) =>
        services.AddDbContext<GeneralContext>(c =>
            c.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.UseNetTopologySuite()));
}
