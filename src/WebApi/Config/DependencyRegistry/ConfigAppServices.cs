namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using IAVH.BioTablero.CM.Application.Interfaces.Services.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Statistics;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.Services.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Users;
using IAVH.BioTablero.CM.Application.Services.Geo;
using IAVH.BioTablero.CM.Application.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Services.Logging;
using IAVH.BioTablero.CM.Application.Services.Resources;
using IAVH.BioTablero.CM.Application.Services.Statistics;
using IAVH.BioTablero.CM.Application.Services.Tag;
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
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IInitiativeContactService, InitiativeContactService>();
        services.AddScoped<IInitiativeLocationService, InitiativeLocationService>();
        services.AddScoped<IInitiativeService, InitiativeService>();
        services.AddScoped<IInitiativeUserService, InitiativeUserService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IInitiativeTagService, InitiativeTagService>();
        services.AddScoped<IJoinRequestService, JoinRequestService>();
        services.AddScoped<IJoinInvitationService, JoinInvitationService>();
        services.AddScoped<IGeneralStatisticsService, GeneralStatisticsService>();
        services.AddScoped<ITerritoryStoryService, TerritoryStoryService>();
        services.AddScoped<ITerritoryStoryImageService, TerritoryStoryImageService>();
        services.AddScoped<ITerritoryStoryVideoService, TerritoryStoryVideoService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IResourceService, ResourceService>();
        services.AddScoped<IResourceTypeService, ResourceTypeService>();
        services.AddScoped<IResourceLinkService, ResourceLinkService>();
        services.AddScoped<IResourceFileService, ResourceFileService>();
        services.AddScoped<IResourceTagService, ResourceTagService>();

        return services;
    }
}
