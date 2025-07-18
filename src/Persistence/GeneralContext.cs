namespace IAVH.BioTablero.CM.Persistence;

using System;
using System.Reflection;

using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using Microsoft.EntityFrameworkCore;

public sealed class GeneralContext : DbContext
{
    public GeneralContext(DbContextOptions<GeneralContext> options)
        : base(options)
    {
        // Patch for Postgres DateTime variables
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        Database.EnsureCreated();
    }

    #region Logs module

    public DbSet<LogDto> Logs { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
