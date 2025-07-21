namespace IAVH.BioTablero.CM.WebApi.Config.SwaggerSetup.Examples.LogsNS;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.DTOs.LogNS;

using Swashbuckle.AspNetCore.Filters;

public class LogOdataResponseExample : IExamplesProvider<Dictionary<string, object>>
{
    /// <summary>
    /// Get examples for entity
    /// </summary>
    /// <returns>Entity examples</returns>
    public Dictionary<string, object> GetExamples() => new()
    {
        ["@odata.count"] = 1,
        ["value"] = new List<LogDto>()
        {
            new(),
        },
    };
}
