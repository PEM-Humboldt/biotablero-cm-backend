namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Filters;

using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Custom ODataQuery filter for Swashbuckle
/// </summary>
public class CustomODataQueryFilter : IOperationFilter
{
    /// <summary>
    /// Apply custom Swashbuckle settings
    /// </summary>
    /// <param name="operation">OpenAPi operation</param>
    /// <param name="context">Swashbuckle operation filter context</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Remove odata params
        var odataParams = context?.ApiDescription.ParameterDescriptions
            .Where(p => p.Type.IsGenericType && p.Type.GetGenericTypeDefinition() == typeof(ODataQueryOptions<>))
            .ToList();

        foreach (var param in odataParams)
        {
            var toRemove = operation?.Parameters
                .FirstOrDefault(p => p.Name == param.Name);
            if (toRemove != null)
            {
                operation.Parameters.Remove(toRemove);
            }
        }

        var isOdataEndpoint = context.MethodInfo.GetParameters()
            .Any(p => p.ParameterType.IsGenericType &&
                      p.ParameterType.GetGenericTypeDefinition() == typeof(ODataQueryOptions<>));

        if (isOdataEndpoint)
        {
            // Add custom params
            operation?.Parameters.Add(new OpenApiParameter
            {
                Name = "$filter",
                In = ParameterLocation.Query,
                Description = "OData filter expression",
                Required = false,
                Schema = new OpenApiSchema { Type = "string" },
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$orderby",
                In = ParameterLocation.Query,
                Description = "OData 'order by' expression",
                Required = false,
                Schema = new OpenApiSchema { Type = "string" },
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$top",
                In = ParameterLocation.Query,
                Description = "OData top count",
                Required = false,
                Schema = new OpenApiSchema { Type = "integer", Format = "int32" },
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$skip",
                In = ParameterLocation.Query,
                Description = "OData skip count",
                Required = false,
                Schema = new OpenApiSchema { Type = "integer", Format = "int32" },
            });

            // Add custom response
            context.SchemaRepository.TryLookupByType(typeof(Dictionary<string, object>), out var schema);

            if (schema == null)
            {
                schema = context.SchemaGenerator.GenerateSchema(typeof(Dictionary<string, object>), context.SchemaRepository);
                schema.AdditionalPropertiesAllowed = false;
                schema.Properties = new Dictionary<string, OpenApiSchema>()
                {
                    {
                        "@odata.count",
                        new OpenApiSchema
                        {
                            Type = "integer",
                            Format = "int32",
                            Default = OpenApiAnyFactory.CreateFromJson("1"),
                        }
                    },
                    {
                        "value",
                        new OpenApiSchema
                        {
                            Type = "array",
                            Items = new OpenApiSchema()
                            {
                                Type = "object",
                                Default = OpenApiAnyFactory.CreateFromJson("{\"id\":1,\"name\":\"string\"}"),
                            },
                        }
                    },
                };
            }

            var content = new Dictionary<string, OpenApiMediaType>()
            {
                {
                    "application/json",
                    new OpenApiMediaType
                    {
                        Schema = schema,
                    }
                },
            };

            operation.Responses.Remove($"{StatusCodes.Status200OK}");
            operation.Responses.Add($"{StatusCodes.Status200OK}", new OpenApiResponse { Content = content });
        }
    }
}
