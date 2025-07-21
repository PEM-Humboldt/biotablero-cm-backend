namespace IAVH.BioTablero.CM.WebApi.Config.SwaggerSetup;

using System;
using System.IO;
using System.Reflection;

using IAVH.BioTablero.CM.WebApi.Config.SwaggerSetup.Filters;

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

        return options;
    }
}
