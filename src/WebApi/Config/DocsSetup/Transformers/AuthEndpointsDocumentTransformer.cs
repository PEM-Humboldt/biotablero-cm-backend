namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Transformers;

using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Utils;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

/// <summary>
/// OpenAPI documentation transformer for authentication setup.
/// </summary>
public sealed class AuthEndpointsDocumentTransformer : IOpenApiDocumentTransformer
{
    private const string SwaggerPropertyClientId = "client_id";
    private const string SwaggerPropertyGrantType = "grant_type";

    private static readonly ISet<OpenApiTagReference> OperationTags =
    new HashSet<OpenApiTagReference>
    {
        new("Auth server"),
    };

    private static readonly IList<OpenApiServer> Servers =
    [
        new()
        {
            Url = $"{EnvUtils.GetRequiredEnv("KC_BASE_URL")}/realms/{EnvUtils.GetRequiredEnv("KC_REALM")}",
        }
    ];

    private static readonly string ClientId =
        EnvUtils.GetRequiredEnv("KC_CLIENT");

    /// <summary>
    /// Applies custom authentication endpoints to the OpenAPI document.
    /// </summary>
    /// <param name="document">OpenAPI document.</param>
    /// <param name="context">OpenAPI document transformer context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        AddPasswordTokenEndpoint(document);
        AddRefreshTokenEndpoint(document);

        return Task.CompletedTask;
    }

    private static void AddPasswordTokenEndpoint(OpenApiDocument document) =>
        document.Paths["/protocol/openid-connect/token?password"] = new OpenApiPathItem
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
                            ["application/x-www-form-urlencoded"] =
                                    new OpenApiMediaType
                                    {
                                        Schema = new OpenApiSchema
                                        {
                                            Type = JsonSchemaType.Object,
                                            Properties =
                                                new Dictionary<string, IOpenApiSchema>
                                                {
                                                    ["username"] = new OpenApiSchema
                                                    {
                                                        Type = JsonSchemaType.String,
                                                        Default = JsonValue.Create(string.Empty),
                                                    },
                                                    ["password"] = new OpenApiSchema
                                                    {
                                                        Type = JsonSchemaType.String,
                                                        Default = JsonValue.Create(string.Empty),
                                                    },
                                                    [SwaggerPropertyClientId] =
                                                        new OpenApiSchema
                                                        {
                                                            Type = JsonSchemaType.String,
                                                            Default = JsonValue.Create(ClientId),
                                                        },
                                                    [SwaggerPropertyGrantType] =
                                                        new OpenApiSchema
                                                        {
                                                            Type = JsonSchemaType.String,
                                                            Default = JsonValue.Create("password"),
                                                        },
                                                },
                                            Required = new HashSet<string>
                                            {
                                                "username",
                                                "password",
                                                SwaggerPropertyClientId,
                                                SwaggerPropertyGrantType,
                                            },
                                        },
                                    },
                        },
                    },
                    Responses = CreateDefaultResponses(),
                },
            },
        };

    private static void AddRefreshTokenEndpoint(OpenApiDocument document) =>
        document.Paths["/protocol/openid-connect/token?refresh"] =
            new OpenApiPathItem
            {
                Description = "Auth server endpoint to obtain a JWT token",
                Servers = Servers,
                Operations = new Dictionary<HttpMethod, OpenApiOperation>
                {
                    [HttpMethod.Post] = new OpenApiOperation
                    {
                        Summary =
                            "Get JWT from auth server with refresh token.",
                        Description =
                            "Authentication using grant_type=refresh_token",
                        Tags = OperationTags,
                        RequestBody = new OpenApiRequestBody
                        {
                            Content =
                                new Dictionary<string, OpenApiMediaType>
                                {
                                    ["application/x-www-form-urlencoded"] =
                                        new OpenApiMediaType
                                        {
                                            Schema = new OpenApiSchema
                                            {
                                                Type = JsonSchemaType.Object,
                                                Properties =
                                                    new Dictionary<string, IOpenApiSchema>
                                                    {
                                                        ["refresh_token"] =
                                                            new OpenApiSchema
                                                            {
                                                                Type =
                                                                    JsonSchemaType.String,
                                                                Default =
                                                                    JsonValue.Create(string.Empty),
                                                            },
                                                        [SwaggerPropertyClientId] =
                                                            new OpenApiSchema
                                                            {
                                                                Type =
                                                                    JsonSchemaType.String,
                                                                Default =
                                                                    JsonValue.Create(ClientId),
                                                            },
                                                        [SwaggerPropertyGrantType] =
                                                            new OpenApiSchema
                                                            {
                                                                Type =
                                                                    JsonSchemaType.String,
                                                                Default =
                                                                    JsonValue.Create("refresh_token"),
                                                            },
                                                    },
                                                Required = new HashSet<string>
                                                {
                                                    "refresh_token",
                                                    SwaggerPropertyClientId,
                                                    SwaggerPropertyGrantType,
                                                },
                                            },
                                        },
                                },
                        },
                        Responses = CreateDefaultResponses(),
                    },
                },
            };

    private static OpenApiResponses CreateDefaultResponses() =>
        new()
        {
            ["200"] = new OpenApiResponse
            {
                Description = "OK",
            },
            ["400"] = new OpenApiResponse
            {
                Description = "Authentication error",
            },
        };
}
