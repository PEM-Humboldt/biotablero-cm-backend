namespace IAVH.BioTablero.CM.Infrastructure.Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;

using Microsoft.EntityFrameworkCore;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

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

    #region Geographic module

    /// <summary>
    /// System logs DbSet
    /// </summary>
    public DbSet<Location> Locations { get; set; }

    #endregion

    #region Initiatives module

    /// <summary>
    /// Initiatives DbSet
    /// </summary>
    public DbSet<Initiative> Initiatives { get; set; }

    /// <summary>
    /// Initiative contacts DbSet
    /// </summary>
    public DbSet<InitiativeContact> InitiativeContacts { get; set; }

    /// <summary>
    /// Initiative locations DbSet
    /// </summary>
    public DbSet<InitiativeLocation> InitiativeLocations { get; set; }

    /// <summary>
    /// Initiative users DbSet
    /// </summary>
    public DbSet<InitiativeUser> InitiativeUsers { get; set; }

    /// <summary>
    /// Initiative user levels DbSet
    /// </summary>
    public DbSet<InitiativeUserLevel> InitiativeUserLevels { get; set; }

    #endregion

    /// <summary>
    /// Configure conventions for custom DbContext
    /// </summary>
    /// <param name="modelBuilder">Database model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder?.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Seeding data
        modelBuilder.Entity<InitiativeUserLevel>().HasData(GetPreconfiguredModules());
    }

    #region Seeding functions

    #region Initiatives user level data

    private static IEnumerable<InitiativeUserLevel> GetPreconfiguredModules()
    {
        var enumData = Enum.GetValues(typeof(InitiativeUserLevelEnum))
            .Cast<InitiativeUserLevelEnum>()
            .Select(t => new InitiativeUserLevel() { Id = (int)t, Name = t.ToString() });

        return enumData;
    }

    #endregion

    #endregion
}
