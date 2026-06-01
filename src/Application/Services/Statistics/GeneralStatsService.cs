namespace IAVH.BioTablero.CM.Application.Services.Statistics;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Statistics;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

/// <summary>
/// General statistics service implementation.
/// </summary>
/// <param name="initiativeRepository">Initiative repository.</param>
public class GeneralStatsService(IInitiativeRepository initiativeRepository) : IGeneralStatsService
{
    private readonly IInitiativeRepository initiativeRepository = initiativeRepository;

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetStatsAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var totalAreaInSquareKm = await initiativeRepository.GetAreaAsync(departmentId, initiativeId, ct);

        return new CustomWebResponse
        {
            ResponseBody = new GeneralStatsDto
            {
                EnabledInitiatives = await initiativeRepository.GetEnabledRecordsCountAsync(departmentId: departmentId, initiativeId: initiativeId, ct: ct),
                PeopleInvolved = await initiativeRepository.GetPeopleInvolvedCountAsync(departmentId, initiativeId, ct),
                AgreementsInvolved = await initiativeRepository.GetAgreementsInvolvedCountAsync(departmentId, initiativeId, ct),
                Area = GeometryUtils.ConvertSquareKilometersToHectares(totalAreaInSquareKm),
            },
        };
    }
}
