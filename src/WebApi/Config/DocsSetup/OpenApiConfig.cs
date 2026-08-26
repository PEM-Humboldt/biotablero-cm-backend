namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Transformers;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

/// <summary>
/// OpenAPI setup.
/// </summary>
public static class OpenApiConfig
{
    /// <summary>
    /// Add OpenApi options.
    /// </summary>
    /// <param name="options">Default OpenApi options.</param>
    /// <returns>Custom OpenApi options.</returns>
    public static OpenApiOptions AddCustomOptions(this OpenApiOptions options)
    {
        // Add general options
        options.AddDocumentTransformer((document, context, ct) =>
        {
            document.Info = new()
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
            };
            return Task.CompletedTask;
        });

        // Enable default security
        options.ConfigDefaultSecurity();

        // Add custom transformers
        options.AddDocumentTransformer<AuthEndpointsDocumentTransformer>();
        options.AddOperationTransformer<ODataQueryTransformer>();
        options.AddOperationTransformer<OpenApiRequestOperationTransformer>();
        options.AddOperationTransformer<OpenApiResponseOperationTransformer>();

        return options;
    }

    /// <summary>
    /// Default OpenAPI security with JWT.
    /// </summary>
    /// <param name="options">OpenApi options.</param>
    private static void ConfigDefaultSecurity(this OpenApiOptions options)
    {
        const string securityDefinitionName = "Bearer";

        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            document.Components.SecuritySchemes.Add(securityDefinitionName, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = securityDefinitionName,
            });

            return Task.CompletedTask;
        });
    }
}
