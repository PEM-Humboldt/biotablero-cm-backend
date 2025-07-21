namespace IAVH.BioTablero.CM.WebApi.Config.SwaggerSetup.Examples.General;

using System.Collections.Generic;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Not found error response example
/// </summary>
public class NotFoundResponseExample : IExamplesProvider<Dictionary<string, string>>
{
    /// <summary>
    /// Get examples for entity
    /// </summary>
    /// <returns>Entity examples</returns>
    public Dictionary<string, string> GetExamples() => new()
    {
        ["error"] = "Not found",
    };
}
