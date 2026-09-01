namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using System;

using Amazon;
using Amazon.Runtime;
using Amazon.S3;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Email;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Iam;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.ImageUtils;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Reports.Services;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Spreadsheets.Services;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Storage;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Video;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Web;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Logging;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Notifications;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Reports;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Email;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Iam.Services;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Iam.TokenProviders;
using IAVH.BioTablero.CM.Infrastructure.Integrations.ImageUtils;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Spreadsheets.Config.Entities;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Spreadsheets.Interfaces;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Spreadsheets.Services;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Video;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Web;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Indicators;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Locations;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Logging;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Notifications;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Reports;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Tags;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.TerritoryStories;

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
        services.AddScoped<IMonitoringEventsRepository, MonitoringEventsRepository>();

        //// Territory Stories
        services.AddScoped<ITerritoryStoryRepository, TerritoryStoryRepository>();
        services.AddScoped<ITerritoryStoryLikeRepository, TerritoryStoryLikeRepository>();
        services.AddScoped<ITerritoryStoryImageRepository, TerritoryStoryImageRepository>();
        services.AddScoped<ITerritoryStoryVideoRepository, TerritoryStoryVideoRepository>();

        //// Resources
        services.AddScoped<IResourceRepository, ResourceRepository>();
        services.AddScoped<IResourceLinkRepository, ResourceLinkRepository>();
        services.AddScoped<IResourceFileRepository, ResourceFileRepository>();
        services.AddScoped<IResourceTagRepository, ResourceTagRepository>();
        services.AddScoped<IResourceLikeRepository, ResourceLikeRepository>();

        //// Notifications
        services.AddScoped<INotificationRepository, NotificationRepository>();

        //// Indicators
        services.AddScoped<IIndicatorRepository, IndicatorRepository>();
        services.AddScoped<IIndicatorVersionRepository, IndicatorVersionRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IIndicatorLocationRepository, IndicatorLocationRepository>();
        services.AddScoped<IIndicatorTagRepository, IndicatorTagRepository>();

        //// Reports
        services.AddScoped<IGeneralStatsRepository, GeneralStatsRepository>();

        // External services
        services.AddScoped(typeof(IReportService<>), typeof(ReportExcelService<>));
        services.AddScoped<IStorageService, StorageService>();

        services.AddHttpClient<IKeycloakTokenProvider, KeycloakTokenProvider>();
        services.AddHttpClient<ICustomApiKeycloakTokenProvider, CustomApiKeycloakTokenProvider>();
        services.AddHttpClient<IIamService, IamService>(client =>
        {
            client.BaseAddress = new Uri($"{EnvUtils.GetRequiredEnv("KC_BASE_URL")}/admin/realms/{EnvUtils.GetRequiredEnv("KC_REALM")}/");
        });
        services.AddHttpClient<IIamCustomApiService, IamCustomApiService>(client =>
        {
            client.BaseAddress = new Uri(EnvUtils.GetRequiredEnv("KC_CUSTOM_API_URL"));
        });

        services.AddSingleton<IEmailService, EmailService>();
        services.AddScoped<IReportConfig<LogDto>, LogReportConfig>();
        services.AddScoped<IIndicatorExcelService, IndicatorExcelService>();
        services.AddScoped<IVideoHelperService, VideoHelperService>();
        services.AddScoped<IImageUtilsService, ImageUtilsService>();
        services.AddScoped<IWebHelperService, WebHelperService>();
        services.AddSingleton<IAmazonS3>(_ =>
        {
            var accessKey = EnvUtils.GetRequiredEnv("AWS_ACCESS_KEY");
            var secretKey = EnvUtils.GetRequiredEnv("AWS_SECRET_KEY");
            var region = EnvUtils.GetRequiredEnv("AWS_REGION");
            var endpoint = EnvUtils.GetRequiredEnv("S3_ENDPOINT_URL");

            var config = new AmazonS3Config()
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(region),
            };

            if (endpoint != null)
            {
                config = new()
                {
                    ServiceURL = endpoint,
                    UseHttp = true,
                    ForcePathStyle = true,
                };
            }

            return new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), config);
        });

        return services;
    }
}
