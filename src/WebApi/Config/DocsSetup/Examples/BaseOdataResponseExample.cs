namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Base OData response example.
/// </summary>
/// <typeparam name="TDto">DTO class type.</typeparam>
public abstract class BaseOdataResponseExample<TDto> : IExamplesProvider<Dictionary<string, object>>
    where TDto : class, IDto, new()
{
    /// <inheritdoc/>
    public Dictionary<string, object> GetExamples() => new()
    {
        ["@odata.count"] = 1,
        ["value"] = new List<TDto>()
        {
            CreateExampleDto(),
        },
    };

    /// <summary>
    /// Create example DTO object.
    /// </summary>
    /// <returns>Example DTO object.</returns>
    protected virtual TDto CreateExampleDto() => new();
}
