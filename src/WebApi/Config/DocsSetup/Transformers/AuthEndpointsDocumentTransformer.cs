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
    private const string OpenApiPropertyClientId = "client_id";
    private const string OpenApiPropertyGrantType = "grant_type";
    private const string OpenApiPropertyUsername = "username";
    private const string OpenApiPropertyPassword = "password";
    private const string OpenApiPropertyRefreshToken = "refresh_token";

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
            Description = "Keycloak Auth Server",
        }
    ];

    private static readonly string ClientId =
        EnvUtils.GetRequiredEnv("KC_CLIENT");

    /// <inheritdoc/>
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
        document.Paths["/protocol/openid-connect/token"] = new OpenApiPathItem
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
                    Servers = Servers,
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
                                                    [OpenApiPropertyUsername] = new OpenApiSchema
                                                    {
                                                        Type = JsonSchemaType.String,
                                                        Default = JsonValue.Create(string.Empty),
                                                    },
                                                    [OpenApiPropertyPassword] = new OpenApiSchema
                                                    {
                                                        Type = JsonSchemaType.String,
                                                        Default = JsonValue.Create(string.Empty),
                                                    },
                                                    [OpenApiPropertyClientId] =
                                                        new OpenApiSchema
                                                        {
                                                            Type = JsonSchemaType.String,
                                                            Default = JsonValue.Create(ClientId),
                                                        },
                                                    [OpenApiPropertyGrantType] =
                                                        new OpenApiSchema
                                                        {
                                                            Type = JsonSchemaType.String,
                                                            Default = JsonValue.Create(OpenApiPropertyPassword),
                                                        },
                                                },
                                            Required = new HashSet<string>
                                            {
                                                OpenApiPropertyUsername,
                                                OpenApiPropertyPassword,
                                                OpenApiPropertyClientId,
                                                OpenApiPropertyGrantType,
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
                        Servers = Servers,
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
                                                        [OpenApiPropertyRefreshToken] =
                                                            new OpenApiSchema
                                                            {
                                                                Type =
                                                                    JsonSchemaType.String,
                                                                Default =
                                                                    JsonValue.Create(string.Empty),
                                                            },
                                                        [OpenApiPropertyClientId] =
                                                            new OpenApiSchema
                                                            {
                                                                Type =
                                                                    JsonSchemaType.String,
                                                                Default =
                                                                    JsonValue.Create(ClientId),
                                                            },
                                                        [OpenApiPropertyGrantType] =
                                                            new OpenApiSchema
                                                            {
                                                                Type =
                                                                    JsonSchemaType.String,
                                                                Default =
                                                                    JsonValue.Create(OpenApiPropertyRefreshToken),
                                                            },
                                                    },
                                                Required = new HashSet<string>
                                                {
                                                    OpenApiPropertyRefreshToken,
                                                    OpenApiPropertyClientId,
                                                    OpenApiPropertyGrantType,
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
