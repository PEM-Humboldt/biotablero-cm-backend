namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using IAVH.BioTablero.CM.Application.Interfaces.Services.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Notifications;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Reports;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.Services.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Users;
using IAVH.BioTablero.CM.Application.Services.Geo;
using IAVH.BioTablero.CM.Application.Services.Indicators;
using IAVH.BioTablero.CM.Application.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Services.Logging;
using IAVH.BioTablero.CM.Application.Services.Notifications;
using IAVH.BioTablero.CM.Application.Services.Reports;
using IAVH.BioTablero.CM.Application.Services.Resources;
using IAVH.BioTablero.CM.Application.Services.Tags;
using IAVH.BioTablero.CM.Application.Services.TerritoryStories;
using IAVH.BioTablero.CM.Application.Services.Users;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Application services dependencies registry.
/// </summary>
public static class ConfigAppServices
{
    /// <summary>
    /// Add custom applications services.
    /// </summary>
    /// <param name="services">Application services.</param>
    /// <returns>Host builder configuration.</returns>
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        // Logs
        services.AddScoped<ILogService, LogService>();

        // locations
        services.AddScoped<ILocationService, LocationService>();

        // Initiatives
        services.AddScoped<IInitiativeContactService, InitiativeContactService>();
        services.AddScoped<IInitiativeLocationService, InitiativeLocationService>();
        services.AddScoped<IInitiativeService, InitiativeService>();
        services.AddScoped<IInitiativeUserService, InitiativeUserService>();
        services.AddScoped<IInitiativeTagService, InitiativeTagService>();
        services.AddScoped<IJoinRequestService, JoinRequestService>();
        services.AddScoped<IJoinInvitationService, JoinInvitationService>();
        services.AddScoped<IMonitoringEventsService, MonitoringEventsService>();

        // Tags
        services.AddScoped<ITagService, TagService>();

        // Territory Stories
        services.AddScoped<ITerritoryStoryService, TerritoryStoryService>();
        services.AddScoped<ITerritoryStoryImageService, TerritoryStoryImageService>();
        services.AddScoped<ITerritoryStoryVideoService, TerritoryStoryVideoService>();

        // Users
        services.AddScoped<IUserService, UserService>();

        // Resources
        services.AddScoped<IResourceService, ResourceService>();
        services.AddScoped<IResourceTypeService, ResourceTypeService>();
        services.AddScoped<IResourceLinkService, ResourceLinkService>();
        services.AddScoped<IResourceFileService, ResourceFileService>();
        services.AddScoped<IResourceTagService, ResourceTagService>();

        // Notifications
        services.AddScoped<INotificationService, NotificationService>();
        services.AddSingleton<ISseNotificationDispatcher, SseNotificationDispatcher>();

        // Indicators
        services.AddScoped<IIndicatorService, IndicatorService>();
        services.AddScoped<IIndicatorVersionService, IndicatorVersionService>();
        services.AddScoped<IIndicatorTagService, IndicatorTagService>();

        // Reports and statistics
        services.AddScoped<IInitiativeStatsService, InitiativeStatsService>();
        services.AddScoped<IGeneralStatsService, GeneralStatsService>();
        services.AddScoped<IReportDataService, ReportDataService>();

        return services;
    }
}
