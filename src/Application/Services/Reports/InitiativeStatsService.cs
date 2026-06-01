namespace IAVH.BioTablero.CM.Application.Services.Reports;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Statistics;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

/// <summary>
/// Initiative statistics service.
/// </summary>
/// <param name="errorTranslator">Error translator.</param>
/// <param name="entityRepository">Monitoring Events repository.</param>
/// <param name="indicatorRepository">Indicator repository.</param>
/// <param name="locationRepository">Location repository.</param>
/// <param name="initiativeRepository">Initiative repository.</param>
public class InitiativeStatsService(
    IValidationErrorTranslator errorTranslator,
    IMonitoringEventsRepository entityRepository,
    IIndicatorRepository indicatorRepository,
    ILocationRepository locationRepository,
    IInitiativeRepository initiativeRepository) : IInitiativeStatsService
{
    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetStats(int initiativeId, CancellationToken ct = default)
    {
        // Validate initiative
        if (!await initiativeRepository.AnyAsync(initiativeId, ct))
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        var totalMunicipalities = await locationRepository.GetMunicipalitiesCountAsync(initiativeId, ct);

        return new()
        {
            ResponseBody = new InitiativeStatsDto()
            {
                TotalMunicipalities = totalMunicipalities > 0 ? totalMunicipalities : 1,
                TotalIndicators = await indicatorRepository.CountAsync(initiativeId, ct),
            },
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetMonitoringEvents(int initiativeId, int? year, CancellationToken ct = default) =>
        new()
        {
            ResponseBody = await entityRepository.GetMonitoringEventsData(initiativeId, year, ct),
        };
}
