namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Logging;

using IAVH.BioTablero.CM.Application.DTOs.Logging;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Log response example.
/// </summary>
public class LogResponseExample : IOpenApiExampleProvider<LogDto>
{
    /// <inheritdoc/>
    public LogDto GetExamples() => new();
}
