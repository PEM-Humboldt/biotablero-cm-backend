namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup;

using System;
using System.IO;
using System.Reflection;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Filters;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// OpenAPI and Swagger UI configuration
/// </summary>
public static class SwaggerConfig
{
    /// <summary>
    /// Add OpenAPI and Swagger UI custom options
    /// </summary>
    /// <param name="options">Default SwaggerGen options</param>
    /// <returns>Custom SwaggerGen options</returns>
    public static SwaggerGenOptions AddCustomOptions(this SwaggerGenOptions options)
    {
        // Add general options
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "0.1.0",
            Title = "BioTableroCM",
            Description = "API for BioTablero's Community Monitoring module",
            Contact = new OpenApiContact
            {
                Name = "Equipo BioTablero",
                Url = new Uri("http://biotablero.humboldt.org.co/"),
                Email = "biotablero@humboldt.org.co",
            },
        });

        // Add xml comments file for docs
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        // Add custom filters
        options.OperationFilter<CustomODataQueryOptions>();

        // Enable example filters
        options.ExampleFilters();

        // Enable default security
        options.ConfigDefaultSecurity();

        return options;
    }

    /// <summary>
    /// Default OpenAPI security
    /// </summary>
    /// <param name="options">Swagger options</param>
    private static void ConfigDefaultSecurity(this SwaggerGenOptions options)
    {
        const string securityDefinitionName = "Bearer";

        options.AddSecurityDefinition(securityDefinitionName, new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer",
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = securityDefinitionName,
                    },
                },
                Array.Empty<string>()
            },
        });
    }
}
