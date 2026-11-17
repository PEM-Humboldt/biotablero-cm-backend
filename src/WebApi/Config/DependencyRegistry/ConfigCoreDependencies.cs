namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using System;

using global::HealthChecks.Network.Core;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.General;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Models.Email;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Email;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Config.DependencyRegistry;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories;
using IAVH.BioTablero.CM.WebApi.Config.HealthChecks;
using IAVH.BioTablero.CM.WebApi.Controllers.Tools;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Core dependencies registry.
/// </summary>
public static class ConfigCoreDependencies
{
    private static readonly string OidcServer = $"{EnvUtils.GetRequiredEnv("KC_BASE_URL")}/realms/{EnvUtils.GetRequiredEnv("KC_REALM")}/";
    private static readonly string ConnectionString = EnvUtils.GetRequiredEnv("CS_MAIN");
    private static readonly Uri IamCustomApiHealthUrl = new($"{EnvUtils.GetRequiredEnv("KC_CUSTOM_API_URL")}/health/ready");
    private static readonly SmtpConfigData SmtpData = EmailService.InitSmtpData();

    /// <summary>
    /// Add core services.
    /// </summary>
    /// <param name="services">Service descriptors collection.</param>
    /// <param name="isDevelopment">Check if development environment is enabled.</param>
    /// <returns>Service descriptors collection with custom services.</returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services, bool isDevelopment = false)
    {
        // Add DB Contexts
        ConfigDbDependencies.AddDbServices(services, ConnectionString);

        services.ConfigureHealthChecks();

        services.AddHttpContextAccessor(); // Required for Serilog (ASP.NET)

        // Enabled MVC without routing
        services.AddMvc(options => options.EnableEndpointRouting = false);

        services.ConfigureFormOptions();

        services.AddMemoryCache();

        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddSingleton<IWebTools, WebTools>();
        services.AddSingleton<IWebViewTools, WebViewTools>();
        services.AddSingleton(typeof(IReadEnumeration<>), typeof(ServiceReadEnumeration<>));

        services.AddAuthService(isDevelopment);

        // Add localization (for custom error messages and codes)
        services.AddLocalization();
        services.AddSingleton<IValidationErrorTranslator, ResxValidationErrorTranslator>();

        return services;
    }

    /// <summary>
    /// Add authentication service.
    /// </summary>
    /// <param name="services">Service descriptors collection.</param>
    /// <param name="isDevelopment">Check if development environment is enabled.</param>
    /// <returns>Service descriptors collection with authentication service.</returns>
    private static IServiceCollection AddAuthService(this IServiceCollection services, bool isDevelopment)
    {
        var clientId = EnvUtils.GetRequiredEnv("KC_CLIENT");
        var useHttpsStr = EnvUtils.GetRequiredEnv("KC_USE_HTTPS");

        if (!bool.TryParse(useHttpsStr, out bool useHttps))
        {
            useHttps = !isDevelopment;
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = OidcServer;
                options.Audience = clientId;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = OidcServer,
                    ValidateLifetime = true,
                };
                options.RequireHttpsMetadata = useHttps;
            });

        return services;
    }

    /// <summary>
    /// Configure Form Options (for upload files).
    /// </summary>
    /// <param name="services">Service descriptors collection.</param>
    /// <returns>Service descriptors collection with custom services.</returns>
    private static IServiceCollection ConfigureFormOptions(this IServiceCollection services) =>
    services
        .Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 5_242_880; // 5 MB
        });

    /// <summary>
    /// Configure HealthChecks.
    /// </summary>
    /// <param name="services">Service descriptors collection.</param>
    /// <returns>Service descriptors collection with custom services.</returns>
    private static IServiceCollection ConfigureHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddOpenIdConnectServer(
                oidcSvrUri: new Uri(OidcServer),
                name: "keycloak",
                tags: ["ready"])
            .AddUrlGroup(
                uri: IamCustomApiHealthUrl,
                name: "keycloak-custom-api",
                tags: ["ready"],
                timeout: TimeSpan.FromSeconds(5))
            .AddNpgSql(
                ConnectionString,
                name: "postgres",
                tags: ["ready"])
            .AddTcpHealthCheck(
                setup: options =>
                {
                    options.AddHost("1.1.1.1", 53);
                },
                name: "internet",
                tags: ["ready"])
            .AddSmtpHealthCheck(
                options =>
                {
                    options.Host = SmtpData.Host;
                    options.Port = SmtpData.Port;
                    options.ConnectionType = SmtpData.EnableSsl
                        ? SmtpConnectionType.TLS
                        : SmtpConnectionType.PLAIN;

                    if (!string.IsNullOrEmpty(SmtpData.User))
                    {
                        options.LoginWith(SmtpData.User, SmtpData.Password);
                    }
                },
                name: "smtp",
                tags: ["ready"])
            .AddCheck<S3HealthCheck>(
                name: "aws s3",
                tags: ["ready"])
            .AddCheck(
                "self",
                () => HealthCheckResult.Healthy(),
                tags: ["live"]);

        return services;
    }
}
