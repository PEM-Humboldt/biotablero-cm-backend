namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Mappings;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
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
        services.AddSingleton<IMapper<LogEntity, LogDto>, LogMappings>();
        services.AddSingleton<IMapper<Location, LocationDto>, LocationMappings>();
        services.AddSingleton<IMapper<Initiative, InitiativeDto>, InitiativeMappings>();
        services.AddSingleton<IMapper<InitiativeContact, InitiativeContactDto>, InitiativeContactMappings>();
        services.AddSingleton<IMapper<InitiativeLocation, InitiativeLocationDto>, InitiativeLocationMappings>();
        services.AddSingleton<IMapper<InitiativeUser, InitiativeUserDto>, InitiativeUserMappings>();
        services.AddSingleton<IMapper<Tag, TagDto>, TagMappings>();
        services.AddSingleton<IMapper<JoinRequest, JoinRequestDto>, JoinRequestMappings>();
        services.AddSingleton<IMapper<JoinInvitation, JoinInvitationDto>, JoinInvitationMappings>();
        services.AddSingleton<IMapper<JoinInvitationGuest, JoinInvitationGuestDto>, JoinInvitationGuestMappings>();
        services.AddSingleton<IMapper<TerritoryStory, TerritoryStoryDto>, TerritoryStoryMappings>();
        services.AddSingleton<IMapper<TerritoryStoryImage, TerritoryStoryImageDto>, TerritoryStoryImageMappings>();
        services.AddSingleton<IMapper<TerritoryStoryVideo, TerritoryStoryVideoDto>, TerritoryStoryVideoMappings>();

        return services;
    }
}
