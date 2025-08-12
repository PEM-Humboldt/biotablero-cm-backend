namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Config.DependencyRegistry;

using System;

using IAVH.BioTablero.CM.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependencies class for main DB context
/// </summary>
public static class ConfigDbDependencies
{
    private static readonly string ConnectionString = Environment.GetEnvironmentVariable("CS_MAIN");

    /// <summary>
    /// Add database services
    /// </summary>
    /// <param name="services">Application services</param>
    public static void AddDbServices(IServiceCollection services) =>
        services.AddDbContext<GeneralContext>(c =>
            c.UseNpgsql(ConnectionString));
}
