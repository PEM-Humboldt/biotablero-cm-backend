namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.Geo;
using IAVH.BioTablero.CM.Application.Mappings.Initiatives;
using IAVH.BioTablero.CM.Application.Mappings.Logging;
using IAVH.BioTablero.CM.Application.Mappings.Resources;
using IAVH.BioTablero.CM.Application.Mappings.Tags;
using IAVH.BioTablero.CM.Application.Mappings.TerritoryStories;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

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
        services.AddSingleton<IMapperCreateAndRead<Tag, TagDto>, TagMappings>();
        services.AddSingleton<IMapperCreateAndRead<Initiative, InitiativeDto>, InitiativeMappings>();
        services.AddSingleton<IMapperCreateAndRead<InitiativeContact, InitiativeContactDto>, InitiativeContactMappings>();
        services.AddSingleton<IMapperCreateAndRead<InitiativeLocation, InitiativeLocationDto>, InitiativeLocationMappings>();
        services.AddSingleton<IMapperCreateAndRead<InitiativeUser, InitiativeUserDto>, InitiativeUserMappings>();
        services.AddSingleton<IMapperCreateAndRead<JoinRequest, JoinRequestDto>, JoinRequestMappings>();
        services.AddSingleton<IMapperCreateAndRead<JoinInvitation, JoinInvitationDto>, JoinInvitationMappings>();
        services.AddSingleton<IMapperCreateAndRead<JoinInvitationGuest, JoinInvitationGuestDto>, JoinInvitationGuestMappings>();
        services.AddSingleton<IMapperCreateAndRead<TerritoryStory, TerritoryStoryDto>, TerritoryStoryMappings>();
        services.AddSingleton<IMapperCreateAndRead<TerritoryStoryImage, TerritoryStoryImageDto>, TerritoryStoryImageMappings>();
        services.AddSingleton<IMapperCreateAndRead<TerritoryStoryVideo, TerritoryStoryVideoDto>, TerritoryStoryVideoMappings>();
        services.AddSingleton<IMapperCreateAndRead<Resource, ResourceDto>, ResourceMappings>();
        services.AddSingleton<IMapperRead<ResourceType, ResourceTypeDto>, ResourceTypeMappings>();
        services.AddSingleton<IMapperCreateAndRead<ResourceFile, ResourceFileDto>, ResourceFileMappings>();
        services.AddSingleton<IMapperCreateAndRead<ResourceLink, ResourceLinkDto>, ResourceLinkMappings>();
        services.AddSingleton<IMapperRead<ResourceTag, ResourceTagDto>, ResourceTagMappings>();

        return services;
    }
}
