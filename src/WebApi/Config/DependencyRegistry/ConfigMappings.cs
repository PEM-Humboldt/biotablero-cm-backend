namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.DTOs.Notifications;
using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.DTOs.Users;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.Geo;
using IAVH.BioTablero.CM.Application.Mappings.Indicators;
using IAVH.BioTablero.CM.Application.Mappings.Initiatives;
using IAVH.BioTablero.CM.Application.Mappings.Logging;
using IAVH.BioTablero.CM.Application.Mappings.Notifications;
using IAVH.BioTablero.CM.Application.Mappings.Resources;
using IAVH.BioTablero.CM.Application.Mappings.Tags;
using IAVH.BioTablero.CM.Application.Mappings.TerritoryStories;
using IAVH.BioTablero.CM.Application.Mappings.Users;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Mappings configuration.
/// </summary>
public static class ConfigMappings
{
    /// <summary>
    /// Add system mappings.
    /// </summary>
    /// <param name="services">Application services.</param>
    /// <returns>Host builder configuration.</returns>
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddSingleton<IMapperRead<LogEntity, LogDto>, LogMappings>();
        services.AddSingleton<IMapperRead<Location, LocationDto>, LocationMappings>();
        services.AddSingleton<IMapperCreateReadAndUpdate<Tag, TagDto>, TagMappings>();
        services.AddSingleton<IMapperCreateReadAndUpdate<Initiative, InitiativeDto>, InitiativeMappings>();
        services.AddSingleton<IMapperCreateReadAndUpdate<InitiativeContact, InitiativeContactDto>, InitiativeContactMappings>();
        services.AddSingleton<IMapperCreateReadAndUpdate<InitiativeLocation, InitiativeLocationDto>, InitiativeLocationMappings>();
        services.AddSingleton<IMapperCreateReadAndUpdate<InitiativeUser, InitiativeUserDto>, InitiativeUserMappings>();
        services.AddSingleton<IMapperRead<InitiativeTag, InitiativeTagDto>, InitiativeTagMappings>();
        services.AddSingleton<IMapperCreateAndRead<JoinRequest, JoinRequestDto>, JoinRequestMappings>();
        services.AddSingleton<IMapperCreateAndRead<JoinInvitation, JoinInvitationDto>, JoinInvitationMappings>();
        services.AddSingleton<IMapperCreateAndRead<JoinInvitationGuest, JoinInvitationGuestDto>, JoinInvitationGuestMappings>();
        services.AddSingleton<IMapperCreateReadAndUpdate<TerritoryStory, TerritoryStoryDto>, TerritoryStoryMappings>();
        services.AddSingleton<IMapperCreateReadAndUpdate<TerritoryStoryImage, TerritoryStoryImageDto>, TerritoryStoryImageMappings>();
        services.AddSingleton<IMapperCreateReadAndUpdate<TerritoryStoryVideo, TerritoryStoryVideoDto>, TerritoryStoryVideoMappings>();
        services.AddSingleton<IMapperCreateReadAndUpdate<Resource, ResourceDto>, ResourceMappings>();
        services.AddSingleton<IMapperRead<ResourceType, ResourceTypeDto>, ResourceTypeMappings>();
        services.AddSingleton<IMapperCreateReadAndUpdate<ResourceFile, ResourceFileDto>, ResourceFileMappings>();
        services.AddSingleton<IMapperCreateReadAndUpdate<ResourceLink, ResourceLinkDto>, ResourceLinkMappings>();
        services.AddSingleton<IMapperRead<ResourceTag, ResourceTagDto>, ResourceTagMappings>();
        services.AddSingleton<IMapperCreateAndRead<Notification, NotificationDto>, NotificationMappings>();
        services.AddSingleton<IMapperRead<ExternalUser, ExternalUserBaseDto>, ExternalUserBaseMappings>();
        services.AddSingleton<IMapperRead<Indicator, IndicatorDto>, IndicatorMappings>();
        services.AddSingleton<IMapperRead<IndicatorTag, IndicatorTagDto>, IndicatorTagMappings>();
        services.AddSingleton<IMapperRead<IndicatorType, IndicatorTypeDto>, IndicatorTypeMappings>();
        services.AddSingleton<IMapperRead<IndicatorLocation, IndicatorLocationDto>, IndicatorLocationMappings>();
        services.AddSingleton<IMapperRead<IndicatorVersion, IndicatorVersionDto>, IndicatorVersionMappings>();

        return services;
    }
}
