namespace IAVH.BioTablero.CM.Application.Services.Reports;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Statistics;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Reports;

/// <summary>
/// General statistics service implementation.
/// </summary>
/// <param name="generalStatsRepository">General statistics repository.</param>
/// <param name="tagMapper">Tag mapper.</param>
public class GeneralStatsService(
    IGeneralStatsRepository generalStatsRepository,
    IMapperCreateReadAndUpdate<Tag, TagDto> tagMapper) : IGeneralStatsService
{
    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetStatsAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var totalAreaInSquareKm = await generalStatsRepository.GetAreaAsync(departmentId, initiativeId, ct);
        var ecosystemsInvolved = await generalStatsRepository.GetEcosystemsAsync(departmentId, initiativeId, ct);

        return new CustomWebResponse
        {
            ResponseBody = new GeneralStatsDto
            {
                EnabledInitiatives = await generalStatsRepository.GetEnabledRecordsCountAsync(departmentId: departmentId, initiativeId: initiativeId, ct: ct),
                PeopleInvolved = await generalStatsRepository.GetPeopleInvolvedCountAsync(departmentId, initiativeId, ct),
                AgreementsInvolved = await generalStatsRepository.GetAgreementsInvolvedCountAsync(departmentId, initiativeId, ct),
                Area = GeometryUtils.ConvertSquareKilometersToHectares(totalAreaInSquareKm),
                EcosystemsInvolved = [.. ecosystemsInvolved.Select(tagMapper.Map)],
            },
        };
    }
}
