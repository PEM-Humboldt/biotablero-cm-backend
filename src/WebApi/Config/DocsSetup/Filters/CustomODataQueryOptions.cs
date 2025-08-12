namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Filters;

using System.Linq;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Custom ODataQueryOptions parameters for Swashbuckle
/// </summary>
public class CustomODataQueryOptions : IOperationFilter
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

        // Add custom params
        if (context.MethodInfo.GetParameters()
            .Any(p => p.ParameterType.IsGenericType &&
                      p.ParameterType.GetGenericTypeDefinition() == typeof(ODataQueryOptions<>)))
        {
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
        }
    }
}
