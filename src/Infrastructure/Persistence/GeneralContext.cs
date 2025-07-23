namespace IAVH.BioTablero.CM.Infrastructure.Persistence;

using System;
using System.Reflection;

using IAVH.BioTablero.CM.Core.Entities.LogNS;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// General database context
/// </summary>
public sealed class GeneralContext : DbContext
{
    /// <summary>
    /// General constructor
    /// /// </summary>
    /// <param name="options">Database context options</param>
    public GeneralContext(DbContextOptions<GeneralContext> options)
        : base(options)
    {
        // Patch for Postgres DateTime variables
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    #region Logs module

    /// <summary>
    /// System logs DbSet
    /// </summary>
    public DbSet<LogEntity> Logs { get; set; }

    #endregion

    /// <summary>
    /// Configure conventions for custom DbContext
    /// </summary>
    /// <param name="modelBuilder">Database model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder?.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
