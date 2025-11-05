namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative geo data response example.
/// </summary>
public class InitiativeGeoDataResponseExample : IExamplesProvider<List<InitiativeCoordinatesDto>>
{
    /// <summary>
    /// Get examples.
    /// </summary>
    /// <returns>Examples.</returns>
    public List<InitiativeCoordinatesDto> GetExamples() => [
        new()
        {
            InitiativeId = 1,
            InitiativeName = "Iniciativa de Prueba",
            Coordinate = [-74.09423914807002, 4.645238678888821],
        },
        new()
        {
            InitiativeId = 2,
            InitiativeName = "Conservacion Amazonas",
            Coordinate = [-74.04999999999998, 4.6499999999999995],
        },
        new()
        {
            InitiativeId = 3,
            InitiativeName = "Reserva Natural Circular",
            Coordinate = [-74.15, 4.6499999999999995],
        },
    ];
}
