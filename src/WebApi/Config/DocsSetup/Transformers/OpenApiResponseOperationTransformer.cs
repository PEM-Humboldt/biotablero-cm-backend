namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Transformers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

/// <summary>
/// OpenAPI operation transformer for response examples.
/// </summary>
public sealed class OpenApiResponseOperationTransformer
    : IOpenApiOperationTransformer
{
    /// <inheritdoc/>
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor
            is not ControllerActionDescriptor controllerAction)
        {
            return Task.CompletedTask;
        }

        var exampleAttribute = controllerAction.MethodInfo
            .GetCustomAttributes(typeof(OpenApiResponseAttribute), true)
            .OfType<OpenApiResponseAttribute>()
            .FirstOrDefault();

        if (exampleAttribute is null)
        {
            return Task.CompletedTask;
        }

        var exampleStatusCode = exampleAttribute.StatusCode;
        var provider = Activator.CreateInstance(exampleAttribute.ProviderType);

        if (provider is null)
        {
            return Task.CompletedTask;
        }

        var getExampleMethod = provider.GetType().GetMethod("GetExample") ??
            throw new InvalidOperationException($"Example provider '{exampleAttribute.ProviderType.FullName}' "
                + "must contain a public GetExample() method.");

        var example = getExampleMethod.Invoke(provider, null);

        var response = new OpenApiResponse
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Example = JsonSerializer.SerializeToNode(example, example?.GetType() ?? typeof(object)),
                },
            },
        };

        operation.Responses ??= [];
        operation.Responses[$"{exampleStatusCode}"] = response;

        return Task.CompletedTask;
    }
}
