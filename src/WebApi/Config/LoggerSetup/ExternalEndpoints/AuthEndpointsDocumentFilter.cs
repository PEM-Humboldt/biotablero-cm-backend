namespace IAVH.BioTablero.CM.WebApi.Config.LoggerSetup.ExternalEndpoints;

using System;
using System.Collections.Generic;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Custom Swagger docs for auth server
/// </summary>
public class AuthEndpointsDocumentFilter : IDocumentFilter
{
    /// <summary>
    /// Apply custom documentation rules
    /// </summary>
    /// <param name="swaggerDoc">OpenAPi document data</param>
    /// <param name="context">Document filter context</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) => swaggerDoc.Paths.Add("/protocol/openid-connect/token", new OpenApiPathItem
    {
        Description = "Auth server endpoint to obtain a JWT token",
        Servers =
        [
            new OpenApiServer { Url = Environment.GetEnvironmentVariable("KC_REALM_URL") }
        ],
        Operations = new Dictionary<OperationType, OpenApiOperation>
        {
            [OperationType.Post] = new OpenApiOperation
            {
                Summary = "Get token from auth server.",
                Description = "Authentication using grant_type=password",
                Tags = [new() { Name = "Auth server" }],
                RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/x-www-form-urlencoded"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    ["username"] = new() { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString(string.Empty) },
                                    ["password"] = new() { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString(string.Empty) },
                                    ["client_id"] = new() { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString(Environment.GetEnvironmentVariable("KC_CLIENT")) },
                                    ["grant_type"] = new() { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString("password") },
                                },
                                Required = new HashSet<string> { "client_id", "username", "password", "grant_type" },
                            },
                        },
                    },
                },
                Responses = new OpenApiResponses
                {
                    ["200"] = new OpenApiResponse { Description = "OK" },
                    ["400"] = new OpenApiResponse { Description = "Authentication error" },
                },
            },
        },
    });
}
