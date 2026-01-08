namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Logging;

using IAVH.BioTablero.CM.Application.DTOs.Logging;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Log response example.
/// </summary>
public class LogResponseExample : IExamplesProvider<LogDto>
{
    /// <inheritdoc/>
    public LogDto GetExamples() => new();
}
