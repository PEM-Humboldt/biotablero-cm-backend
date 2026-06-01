namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Demographic statistics response example.
/// </summary>
public class DemographicStatsResponseExample : IExamplesProvider<DemographicStatsDto>
{
    /// <inheritdoc/>
    public DemographicStatsDto GetExamples() =>
        new()
        {
            Gender =
            [
                new("Femenino", 1),
                new("Masculino", 1),
            ],
            Organization =
            [
                new("Institución pública", 1),
                new("Empresa privada", 1),
            ],
            SelfRecognition =
            [
                new("Indígena", 1),
                new("Campesino", 1),
            ],
        };
}
