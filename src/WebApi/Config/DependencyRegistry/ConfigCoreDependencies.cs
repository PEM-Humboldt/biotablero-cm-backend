namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.General;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Config.DependencyRegistry;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories;
using IAVH.BioTablero.CM.WebApi.Controllers.Tools;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Core dependencies registry.
/// </summary>
public static class ConfigCoreDependencies
{
    private static readonly string OidcServer = $"{Environment.GetEnvironmentVariable("KC_BASE_URL")}/realms/{Environment.GetEnvironmentVariable("KC_REALM")}/";
    private static readonly string ConnectionString = Environment.GetEnvironmentVariable("CS_MAIN");

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

        // Health checks setup
        services.AddHealthChecks()
            .AddOpenIdConnectServer(
                oidcSvrUri: new Uri(OidcServer),
                name: "keycloak")
            .AddNpgSql(
                ConnectionString,
                name: "postgres");

        services.AddHttpContextAccessor(); // Required for Serilog (ASP.NET)

        // Enabled MVC without routing
        services.AddMvc(options => options.EnableEndpointRouting = false);

        services.ConfigureFormOptions();

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
        var clientId = Environment.GetEnvironmentVariable("KC_CLIENT");

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
                options.RequireHttpsMetadata = !isDevelopment;
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
}
