namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Logging;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Email;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Iam;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Config.Entities;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Interfaces;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Video;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Locations;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Logging;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// External services dependencies registry.
/// </summary>
public static class ConfigExternalServices
{
    /// <summary>
    /// Add custom external services.
    /// </summary>
    /// <param name="services">Application services.</param>
    /// <returns>Host builder configuration.</returns>
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        // Repositories

        //// Logs
        services.AddScoped<ILogRepository, LogRepository>();

        // Locations
        services.AddScoped<ILocationPolygonRepository, LocationPolygonRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();

        //// Initiatives
        services.AddScoped<IInitiativeRepository, InitiativeRepository>();
        services.AddScoped<IInitiativeContactRepository, InitiativeContactRepository>();
        services.AddScoped<IInitiativeLocationRepository, InitiativeLocationRepository>();
        services.AddScoped<IInitiativeTagRepository, InitiativeTagRepository>();
        services.AddScoped<IInitiativeUserRepository, InitiativeUserRepository>();
        services.AddScoped<IJoinInvitationRepository, JoinInvitationRepository>();
        services.AddScoped<IJoinRequestRepository, JoinRequestRepository>();
        services.AddScoped<ITagRepository, TagRepository>();

        // External services
        services.AddScoped(typeof(IReportService<>), typeof(ReportExcelService<>));
        services.AddScoped<IStorageService, StorageService>();
        services.AddSingleton<IIamService, IamService>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddScoped<IReportConfig<LogDto>, LogReportConfig>();
        services.AddScoped<IVideoHelperService, VideoHelperService>();

        return services;
    }
}
