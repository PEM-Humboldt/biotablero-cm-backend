namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Filters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

/// <summary>
/// Custom OpenAPI transformer for OData query options.
/// </summary>
public sealed class CustomODataQueryFilter : IOpenApiOperationTransformer
{
    /// <summary>
    /// Transforms an OpenAPI operation to customize OData query parameters
    /// and responses.
    /// </summary>
    /// <param name="operation">OpenAPI operation.</param>
    /// <param name="context">OpenAPI operation transformer context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor is not ControllerActionDescriptor controllerAction)
        {
            return Task.CompletedTask;
        }

        var methodInfo = controllerAction.MethodInfo;

        var isOdataEndpoint = methodInfo
            .GetParameters()
            .Any(p =>
                p.ParameterType.IsGenericType &&
                p.ParameterType.GetGenericTypeDefinition() == typeof(ODataQueryOptions<>));

        if (!isOdataEndpoint)
        {
            return Task.CompletedTask;
        }

        var isReportEndpoint = methodInfo.Name.Contains("Report", StringComparison.InvariantCultureIgnoreCase);

        if (isReportEndpoint)
        {
            return Task.CompletedTask;
        }

        // Remove automatically generated OData parameters.
        if (operation.Parameters is not null)
        {
            var parametersToRemove = operation.Parameters
                .Where(p =>
                    p.Name is "$filter" or "$orderby" or "$top" or "$skip")
                .ToList();

            foreach (var parameter in parametersToRemove)
            {
                operation.Parameters.Remove(parameter);
            }
        }

        operation.Parameters ??= [];

        // $filter
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "$filter",
            In = ParameterLocation.Query,
            Description = "OData filter expression",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String,
            },
        });

        // $orderby
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "$orderby",
            In = ParameterLocation.Query,
            Description = "OData 'order by' expression",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String,
            },
        });

        // $top
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "$top",
            In = ParameterLocation.Query,
            Description = "OData top count",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.Integer,
                Format = "int32",
            },
        });

        // $skip
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "$skip",
            In = ParameterLocation.Query,
            Description = "OData skip count",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.Integer,
                Format = "int32",
            },
        });

        // Custom OData response.
        operation.Responses ??= [];

        operation.Responses.Remove("200");

        operation.Responses["200"] = new OpenApiResponse
        {
            Description = "OK",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = JsonSchemaType.Object,
                        Properties = new Dictionary<string, IOpenApiSchema>
                        {
                            ["@odata.count"] = new OpenApiSchema
                            {
                                Type = JsonSchemaType.Integer,
                                Format = "int32",
                                Default = JsonValue.Create(1),
                            },
                            ["value"] = new OpenApiSchema
                            {
                                Type = JsonSchemaType.Array,
                                Items = new OpenApiSchema
                                {
                                    Type = JsonSchemaType.Object,
                                    Default = JsonNode.Parse("""{"id":1,"name":"string"}"""),
                                },
                            },
                        },
                    },
                },
            },
        };

        return Task.CompletedTask;
    }
}
