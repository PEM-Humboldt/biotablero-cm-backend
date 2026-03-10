namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using System;

using Amazon;
using Amazon.Runtime;
using Amazon.S3;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Logging;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Email;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Iam;
using IAVH.BioTablero.CM.Infrastructure.Integrations.ImageUtils;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Config.Entities;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Interfaces;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Video;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Web;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Locations;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Logging;
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

        // External services
        services.AddScoped(typeof(IReportService<>), typeof(ReportExcelService<>));
        services.AddScoped<IStorageService, StorageService>();
        services.AddSingleton<IIamService, IamService>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddScoped<IReportConfig<LogDto>, LogReportConfig>();
        services.AddScoped<IVideoHelperService, VideoHelperService>();
        services.AddScoped<IImageUtilsService, ImageUtilsService>();
        services.AddScoped<IWebHelperService, WebHelperService>();
        services.AddSingleton<IAmazonS3>(_ =>
        {
            var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
            var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
            var region = Environment.GetEnvironmentVariable("AWS_REGION");
            var endpoint = Environment.GetEnvironmentVariable("S3_ENDPOINT_URL");

            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(region),
                ServiceURL = endpoint,
                UseHttp = true,
                ForcePathStyle = true,
            };

            return new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), config);
        });

        return services;
    }
}
