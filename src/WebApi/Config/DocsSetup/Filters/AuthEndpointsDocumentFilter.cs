namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Filters;

using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Nodes;

using IAVH.BioTablero.CM.Application.Utils;

using Microsoft.OpenApi;

using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Custom Swagger docs for auth server.
/// </summary>
public class AuthEndpointsDocumentFilter : IDocumentFilter
{
    private const string SwaggerPropertyClientId = "client_id";
    private const string SwaggerPropertyGrantType = "grant_type";
    private static readonly ISet<OpenApiTagReference> OperationTags = new HashSet<OpenApiTagReference> { new("Auth server") };

    private static readonly OpenApiResponses DefaultResponses = new()
    {
        ["200"] = new OpenApiResponse { Description = "OK" },
        ["400"] = new OpenApiResponse { Description = "Authentication error" },
    };

    private static readonly IList<OpenApiServer> Servers =
    [
        new() { Url = $"{EnvUtils.GetRequiredEnv("KC_BASE_URL")}/realms/{EnvUtils.GetRequiredEnv("KC_REALM")}" },
    ];

    private static readonly string ClientId = EnvUtils.GetRequiredEnv("KC_CLIENT");

    /// <summary>
    /// Apply custom documentation rules.
    /// </summary>
    /// <param name="swaggerDoc">OpenAPi document data.</param>
    /// <param name="context">Document filter context.</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Paths.Add("/protocol/openid-connect/token?password", new OpenApiPathItem
        {
            Description = "Auth server endpoint to obtain a JWT token",
            Servers = Servers,
            Operations = new Dictionary<HttpMethod, OpenApiOperation>
            {
                [HttpMethod.Post] = new OpenApiOperation
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
                                    Type = JsonSchemaType.Object,
                                    Properties = new Dictionary<string, IOpenApiSchema>
                                    {
                                        ["username"] = new OpenApiSchema { Type = JsonSchemaType.String, Default = JsonValue.Create(string.Empty) },
                                        ["password"] = new OpenApiSchema { Type = JsonSchemaType.String, Default = JsonValue.Create(string.Empty) },
                                        [SwaggerPropertyClientId] = new OpenApiSchema { Type = JsonSchemaType.String, Default = JsonValue.Create(ClientId) },
                                        [SwaggerPropertyGrantType] = new OpenApiSchema { Type = JsonSchemaType.String, Default = JsonValue.Create("password") },
                                    },
                                    Required = new HashSet<string> { "username", "password", SwaggerPropertyClientId, SwaggerPropertyGrantType },
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
            Operations = new Dictionary<HttpMethod, OpenApiOperation>
            {
                [HttpMethod.Post] = new OpenApiOperation
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
                                    Type = JsonSchemaType.Object,
                                    Properties = new Dictionary<string, IOpenApiSchema>
                                    {
                                        ["refresh_token"] = new OpenApiSchema { Type = JsonSchemaType.String, Default = JsonValue.Create(string.Empty) },
                                        [SwaggerPropertyClientId] = new OpenApiSchema { Type = JsonSchemaType.String, Default = JsonValue.Create(ClientId) },
                                        [SwaggerPropertyGrantType] = new OpenApiSchema { Type = JsonSchemaType.String, Default = JsonValue.Create("refresh_token") },
                                    },
                                    Required = new HashSet<string> { "refresh_token", SwaggerPropertyClientId, SwaggerPropertyGrantType },
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
