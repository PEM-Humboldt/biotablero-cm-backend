namespace IAVH.BioTablero.CM.Infrastructure.Persistence;

using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

/// <summary>
/// Design-time factory for <see cref="GeneralContext"/>, used by EF Core tools (migrations, bundle).
/// Reads CS_MAIN from the environment; falls back to a placeholder when no connection is available.
/// </summary>
public sealed class GeneralContextFactory : IDesignTimeDbContextFactory<GeneralContext>
{
    /// <summary>
    /// Creates a <see cref="GeneralContext"/> instance for design-time operations.
    /// </summary>
    /// <param name="args">Command-line arguments (unused).</param>
    /// <returns>A configured <see cref="GeneralContext"/> instance.</returns>
    public GeneralContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("CS_MAIN")
            ?? "Host=localhost;Port=5432;Username=dev;Password=dev;Database=dev";

        var options = new DbContextOptionsBuilder<GeneralContext>()
            .UseNpgsql(connectionString, o => o.UseNetTopologySuite())
            .Options;

        return new GeneralContext(options);
    }
}
