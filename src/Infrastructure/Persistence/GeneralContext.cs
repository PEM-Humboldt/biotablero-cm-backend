namespace IAVH.BioTablero.CM.Infrastructure.Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

using Microsoft.EntityFrameworkCore;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;
using JoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.JoinRequestStatus;
using TagCategoryEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.TagEnums.TagCategory;

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
        // Patch for Postgres DateTimeOffset variables
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    #region Logs module

    /// <summary>
    /// System logs DbSet.
    /// </summary>
    public DbSet<LogEntity> Logs { get; set; }

    #endregion

    #region Notifications module

    /// <summary>
    /// Notifications DbSet.
    /// </summary>
    public DbSet<Notification> Notifications { get; set; }

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
    /// Initiative tags DbSet.
    /// </summary>
    public DbSet<InitiativeTag> InitiativeTags { get; set; }

    /// <summary>
    /// Tags DbSet.
    /// </summary>
    public DbSet<Tag> Tags { get; set; }

    /// <summary>
    /// Tag categories DbSet.
    /// </summary>
    public DbSet<TagCategory> TagCategories { get; set; }

    /// <summary>
    /// Join requests DbSet.
    /// </summary>
    public DbSet<JoinRequest> JoinRequests { get; set; }

    /// <summary>
    /// Join request statuses DbSet.
    /// </summary>
    public DbSet<JoinRequestStatus> JoinRequestsStatuses { get; set; }

    /// <summary>
    /// Join invitations DbSet.
    /// </summary>
    public DbSet<JoinInvitation> JoinInvitations { get; set; }

    /// <summary>
    /// Join invitation guests DbSet.
    /// </summary>
    public DbSet<JoinInvitationGuest> JoinInvitationGuests { get; set; }

    #endregion

    #region Territory Story entities

    /// <summary>
    /// Territory stories DbSet.
    /// </summary>
    public DbSet<TerritoryStory> TerritoryStories { get; set; }

    /// <summary>
    /// Territory story likes DbSet.
    /// </summary>
    public DbSet<TerritoryStoryLike> TerritoryStoryLikes { get; set; }

    /// <summary>
    /// Territory story images DbSet.
    /// </summary>
    public DbSet<TerritoryStoryImage> TerritoryStoryImages { get; set; }

    /// <summary>
    /// Territory story videos DbSet.
    /// </summary>
    public DbSet<TerritoryStoryVideo> TerritoryStoryVideos { get; set; }

    #endregion

    #region Resource entities

    /// <summary>
    /// Resource DbSet.
    /// </summary>
    public DbSet<Resource> Resources { get; set; }

    /// <summary>
    /// Resource Type DbSet.
    /// </summary>
    public DbSet<ResourceType> ResourceTypes { get; set; }

    /// <summary>
    /// Resource File DbSet.
    /// </summary>
    public DbSet<ResourceFile> ResourceFiles { get; set; }

    /// <summary>
    /// Resource Link DbSet.
    /// </summary>
    public DbSet<ResourceLink> ResourceLinks { get; set; }

    /// <summary>
    /// Resource Tag DbSet.
    /// </summary>
    public DbSet<ResourceTag> ResourceTags { get; set; }

    /// <summary>
    /// Resource Like DbSet.
    /// </summary>
    public DbSet<ResourceLike> ResourceLikes { get; set; }

    #endregion

    #region Indicator entities

    /// <summary>
    /// Indicator DbSet.
    /// </summary>
    public DbSet<Indicator> Indicators { get; set; }

    /// <summary>
    /// Indicator Type DbSet.
    /// </summary>
    public DbSet<IndicatorType> IndicatorTypes { get; set; }

    /// <summary>
    /// Indicator Tag DbSet.
    /// </summary>
    public DbSet<IndicatorTag> IndicatorTags { get; set; }

    /// <summary>
    /// Indicator Location DbSet.
    /// </summary>
    public DbSet<IndicatorLocation> IndicatorLocations { get; set; }

    /// <summary>
    /// Indicator Version DbSet.
    /// </summary>
    public DbSet<IndicatorVersion> IndicatorVersions { get; set; }

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
        modelBuilder.Entity<TagCategory>().HasData(GetDefaultTagCategories());
        modelBuilder.Entity<InitiativeUserLevel>().HasData(GetDefaultInitiativeUserLevels());
        modelBuilder.Entity<JoinRequestStatus>().HasData(GetDefaultJoinRequestStatuses());
    }

    #region Seeding functions

    #region Tags data

    /// <summary>
    /// Get default tag categories.
    /// </summary>
    /// <returns>Default tag categories list.</returns>
    private static IEnumerable<TagCategory> GetDefaultTagCategories()
    {
        var enumData = Enum.GetValues(typeof(TagCategoryEnum))
            .Cast<TagCategoryEnum>()
            .Select(t => new TagCategory() { Id = (int)t, Name = t.ToString() });

        return enumData;
    }

    #endregion

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
    /// Get default join request statuses.
    /// </summary>
    /// <returns>Default join request statuses list.</returns>
    private static IEnumerable<JoinRequestStatus> GetDefaultJoinRequestStatuses()
    {
        var enumData = Enum.GetValues(typeof(JoinRequestStatusEnum))
            .Cast<JoinRequestStatusEnum>()
            .Select(t => new JoinRequestStatus() { Id = (int)t, Name = t.ToString() });

        return enumData;
    }

    #endregion

    #endregion
}
