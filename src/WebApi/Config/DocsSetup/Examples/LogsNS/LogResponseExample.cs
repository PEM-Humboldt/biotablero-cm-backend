namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.LogsNS;

using IAVH.BioTablero.CM.Application.DTOs.Logging;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Log response example
/// </summary>
public class LogResponseExample : IExamplesProvider<LogDto>
{
    /// <summary>
    /// Get examples for entity
    /// </summary>
    /// <returns>Entity examples</returns>
    public LogDto GetExamples() => new();
}
