using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IAVH.BioTablero.CM.Persistence;

public static class Dependencies
{
    private static readonly string ConnectionString = Environment.GetEnvironmentVariable("CS_MAIN");

    public static void ConfigureServices(IConfiguration _, IServiceCollection services) =>
        services.AddDbContext<GeneralContext>(c =>
            c.UseNpgsql(ConnectionString)
        );
}
