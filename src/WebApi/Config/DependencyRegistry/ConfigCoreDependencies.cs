namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories;
using IAVH.BioTablero.CM.WebApi.Controllers.Tools;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Core dependencies registry.
/// </summary>
public static class ConfigCoreDependencies
{
    /// <summary>
    /// Add core services.
    /// </summary>
    /// <param name="services">Service descriptors collection.</param>
    /// <param name="isDevelopment">Check if development environment is enabled.</param>
    /// <returns>Service descriptors collection with custom services.</returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services, bool isDevelopment = false)
    {
        services.AddHealthChecks();
        services.AddHttpContextAccessor(); // Required for Serilog (ASP.NET)

        services.ConfigureFormOptions();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<IWebTools, WebTools>();
        services.AddSingleton(typeof(IReadEnumeration<>), typeof(ServiceReadEnumeration<>));

        services.AddAuthService(isDevelopment);

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
        var url = $"{Environment.GetEnvironmentVariable("KC_BASE_URL")}/realms/{Environment.GetEnvironmentVariable("KC_REALM")}";
        var clientId = Environment.GetEnvironmentVariable("KC_CLIENT");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = url;
                options.Audience = clientId;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = url,
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
            options.MultipartBodyLengthLimit = 10_000_000; // 10 MB
        });
}
