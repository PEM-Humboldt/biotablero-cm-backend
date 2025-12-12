namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Validators.Initiatives;
using IAVH.BioTablero.CM.Application.Validators.Tags;
using IAVH.BioTablero.CM.Application.Validators.TerritoryStory;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Validators configuration.
/// </summary>
public static class ConfigValidators
{
    /// <summary>
    /// Add system validators.
    /// </summary>
    /// <param name="services">Application services.</param>
    /// <returns>Host builder configuration.</returns>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<InitiativeValidator>();
        services.AddValidatorsFromAssemblyContaining<InitiativeContactValidator>();
        services.AddValidatorsFromAssemblyContaining<InitiativeUserValidator>();
        services.AddValidatorsFromAssemblyContaining<InitiativeLocationValidator>();
        services.AddValidatorsFromAssemblyContaining<TagValidator>();
        services.AddValidatorsFromAssemblyContaining<JoinInvitationValidator>();
        services.AddValidatorsFromAssemblyContaining<TerritoryStoryValidator>();
        services.AddValidatorsFromAssemblyContaining<TerritoryStoryImageValidator>();
        services.AddValidatorsFromAssemblyContaining<TerritoryStoryVideoValidator>();

        return services;
    }
}
