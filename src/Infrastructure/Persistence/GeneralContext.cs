namespace IAVH.BioTablero.CM.Infrastructure.Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;

using Microsoft.EntityFrameworkCore;

using InitiativeTagCategoryEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeTagCategory;
using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// General database context.
/// </summary>
public sealed class GeneralContext : DbContext
{
    /// <summary>
    /// General constructor.
    /// </summary>
    /// <param name="options">Database context options.</param>
    public GeneralContext(DbContextOptions<GeneralContext> options)
        : base(options)
    {
        // Patch for Postgres DateTime variables
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    #region Logs module

    /// <summary>
    /// System logs DbSet.
    /// </summary>
    public DbSet<LogEntity> Logs { get; set; }

    #endregion

    #region Geographic module

    /// <summary>
    /// Locations DbSet.
    /// </summary>
    public DbSet<Location> Locations { get; set; }

    /// <summary>
    /// Location Polygons DbSet.
    /// </summary>
    public DbSet<LocationPolygon> LocationPolygons { get; set; }

    #endregion

    #region Initiatives module

    /// <summary>
    /// Initiatives DbSet.
    /// </summary>
    public DbSet<Initiative> Initiatives { get; set; }

    /// <summary>
    /// Initiative contacts DbSet.
    /// </summary>
    public DbSet<InitiativeContact> InitiativeContacts { get; set; }

    /// <summary>
    /// Initiative locations DbSet.
    /// </summary>
    public DbSet<InitiativeLocation> InitiativeLocations { get; set; }

    /// <summary>
    /// Initiative users DbSet.
    /// </summary>
    public DbSet<InitiativeUser> InitiativeUsers { get; set; }

    /// <summary>
    /// Initiative user levels DbSet.
    /// </summary>
    public DbSet<InitiativeUserLevel> InitiativeUserLevels { get; set; }

    /// <summary>
    /// Initiative tag initiatives DbSet.
    /// </summary>
    public DbSet<InitiativeTagInitiative> InitiativeTagInitiatives { get; set; }

    /// <summary>
    /// Initiative tags DbSet.
    /// </summary>
    public DbSet<InitiativeTag> InitiativeTags { get; set; }

    /// <summary>
    /// Initiative tag categories DbSet.
    /// </summary>
    public DbSet<InitiativeTagCategory> InitiativeTagCategories { get; set; }

    #endregion

    /// <summary>
    /// Configure conventions for custom DbContext.
    /// </summary>
    /// <param name="modelBuilder">Database model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder?.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Seeding data
        modelBuilder.Entity<InitiativeUserLevel>().HasData(GetDefaultInitiativeUserLevels());
        modelBuilder.Entity<InitiativeTagCategory>().HasData(GetDefaultInitiativeTagCategories());
    }

    #region Seeding functions

    #region Initiatives data

    /// <summary>
    /// Get default initative user levels.
    /// </summary>
    /// <returns>Default initative user levels list.</returns>
    private static IEnumerable<InitiativeUserLevel> GetDefaultInitiativeUserLevels()
    {
        var enumData = Enum.GetValues(typeof(InitiativeUserLevelEnum))
            .Cast<InitiativeUserLevelEnum>()
            .Select(t => new InitiativeUserLevel() { Id = (int)t, Name = t.ToString() });

        return enumData;
    }

    /// <summary>
    /// Get default initative tag categories.
    /// </summary>
    /// <returns>Default initative tag categories list.</returns>
    private static IEnumerable<InitiativeTagCategory> GetDefaultInitiativeTagCategories()
    {
        var enumData = Enum.GetValues(typeof(InitiativeTagCategoryEnum))
            .Cast<InitiativeTagCategoryEnum>()
            .Select(t => new InitiativeTagCategory() { Id = (int)t, Name = t.ToString() });

        return enumData;
    }

    #endregion

    #endregion
}
