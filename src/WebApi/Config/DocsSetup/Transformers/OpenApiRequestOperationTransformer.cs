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

        var provider = Activator.CreateInstance(exampleAttribute.ProviderType);

        if (provider is null)
        {
            return Task.CompletedTask;
        }

        var getExampleMethod = provider.GetType().GetMethod("GetExample") ??
            throw new InvalidOperationException($"Example provider '{exampleAttribute.ProviderType.FullName}' "
                + "must contain a public GetExample() method.");

        var example = getExampleMethod.Invoke(provider, null);

        operation.RequestBody ??= new OpenApiRequestBody();
        operation.RequestBody.Content!.Add("application/json", new OpenApiMediaType
        {
            Example = JsonSerializer.SerializeToNode(example, example?.GetType() ?? typeof(object)),
        });

        return Task.CompletedTask;
    }
}
