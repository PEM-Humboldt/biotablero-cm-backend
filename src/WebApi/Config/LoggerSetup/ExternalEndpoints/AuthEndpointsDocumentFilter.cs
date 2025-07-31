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
    private static readonly IList<OpenApiTag> OperationTags = [new() { Name = "Auth server" }];

    private static readonly OpenApiResponses DefaultResponses = new()
    {
        ["200"] = new OpenApiResponse { Description = "OK" },
        ["400"] = new OpenApiResponse { Description = "Authentication error" },
    };

    private static readonly IList<OpenApiServer> Servers =
    [
        new() { Url = Environment.GetEnvironmentVariable("KC_REALM_URL") },
    ];

    private static readonly string ClientId = Environment.GetEnvironmentVariable("KC_CLIENT");

    /// <summary>
    /// Apply custom documentation rules
    /// </summary>
    /// <param name="swaggerDoc">OpenAPi document data</param>
    /// <param name="context">Document filter context</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Paths.Add("/protocol/openid-connect/token?password", new OpenApiPathItem
        {
            Description = "Auth server endpoint to obtain a JWT token",
            Servers = Servers,
            Operations = new Dictionary<OperationType, OpenApiOperation>
            {
                [OperationType.Post] = new OpenApiOperation
                {
                    Summary = "Get JWT from auth server with password.",
                    Description = "Authentication using grant_type=password",
                    Tags = OperationTags,
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
                                        ["client_id"] = new() { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString(ClientId) },
                                        ["grant_type"] = new() { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString("password") },
                                    },
                                    Required = new HashSet<string> { "username", "password", "client_id", "grant_type" },
                                },
                            },
                        },
                    },
                    Responses = DefaultResponses,
                },
            },
        });

        swaggerDoc.Paths.Add("/protocol/openid-connect/token?refresh", new OpenApiPathItem
        {
            Description = "Auth server endpoint to obtain a JWT token",
            Servers = Servers,
            Operations = new Dictionary<OperationType, OpenApiOperation>
            {
                [OperationType.Post] = new OpenApiOperation
                {
                    Summary = "Get JWT from auth server with refresh token.",
                    Description = "Authentication using grant_type=refresh_token",
                    Tags = OperationTags,
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
                                        ["refresh_token"] = new() { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString(string.Empty) },
                                        ["client_id"] = new() { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString(ClientId) },
                                        ["grant_type"] = new() { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString("refresh_token") },
                                    },
                                    Required = new HashSet<string> { "refresh_token", "client_id", "grant_type" },
                                },
                            },
                        },
                    },
                    Responses = DefaultResponses,
                },
            },
        });
    }
}
