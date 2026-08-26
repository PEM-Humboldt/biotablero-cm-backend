namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Transformers;

using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

/// <summary>
/// OpenAPI operation transformer for request examples.
/// </summary>
public sealed class OpenApiRequestOperationTransformer
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
            .GetCustomAttributes(typeof(OpenApiRequestAttribute), true)
            .OfType<OpenApiRequestAttribute>()
            .FirstOrDefault();

        if (exampleAttribute is null)
        {
            return Task.CompletedTask;
        }

        if (operation.RequestBody?.Content is null)
        {
            return Task.CompletedTask;
        }

        var provider = Activator.CreateInstance(exampleAttribute.ProviderType) ??
            throw new InvalidOperationException($"Could not create OpenAPI example provider " +
                $"'{exampleAttribute.ProviderType.FullName}'.");

        var getExampleMethod = provider.GetType().GetMethod("GetExamples") ??
            throw new InvalidOperationException($"Example provider '{exampleAttribute.ProviderType.FullName}' "
                + "must contain a public GetExamples() method.");

        var example = getExampleMethod.Invoke(provider, null);

        operation.RequestBody ??= new OpenApiRequestBody();
        operation.RequestBody.Content!["application/json"] = new OpenApiMediaType
        {
            Example = JsonSerializer.SerializeToNode(example, example?.GetType() ?? typeof(object)),
        };

        // Remove unused content types
        foreach (var (contentType, _) in operation.RequestBody.Content)
        {
            if (contentType != "application/json" && contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                operation.RequestBody.Content.Remove(contentType);
            }
        }

        return Task.CompletedTask;
    }
}
