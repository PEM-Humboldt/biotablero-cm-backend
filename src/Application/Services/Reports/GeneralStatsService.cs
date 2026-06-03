namespace IAVH.BioTablero.CM.Application.Services.Reports;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Reports;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Reports;

/// <summary>
/// General statistics service implementation.
/// </summary>
/// <param name="generalStatsRepository">General statistics repository.</param>
/// <param name="tagMapper">Tag mapper.</param>
/// <param name="iamService">IAM Service.</param>
public class GeneralStatsService(
    IGeneralStatsRepository generalStatsRepository,
    IMapperCreateReadAndUpdate<Tag, TagDto> tagMapper,
    IIamService iamService) : IGeneralStatsService
{
    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetGeneralStatsAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var totalAreaInSquareKm = await generalStatsRepository.GetAreaAsync(departmentId, initiativeId, ct);

        return new CustomWebResponse
        {
            ResponseBody = new GeneralStatsDto
            {
                EnabledInitiatives = await generalStatsRepository.GetEnabledRecordsCountAsync(departmentId: departmentId, initiativeId: initiativeId, ct: ct),
                PeopleInvolved = await generalStatsRepository.GetPeopleInvolvedCountAsync(departmentId, initiativeId, ct),
                AgreementsInvolved = await generalStatsRepository.GetAgreementsInvolvedCountAsync(departmentId, initiativeId, ct),
                Area = GeometryUtils.ConvertSquareKilometersToHectares(totalAreaInSquareKm),
            },
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetEcosystemsStatsAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var ecosystemsInvolved = await generalStatsRepository.GetEcosystemsAsync(departmentId, initiativeId, ct);

        return new CustomWebResponse
        {
            ResponseBody = new EcosystemsStatsDto
            {
                EcosystemsInvolved = [.. ecosystemsInvolved.Select(tagMapper.Map)],
            },
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetDemographicStats(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var externalUsersData = await iamService.GetAllEnabledUsersDataAsync(ct);
        var internalUsersData = await generalStatsRepository.GetUserNamesAsync(departmentId, initiativeId, ct);
        var filteredUsersData = externalUsersData.Where(e => internalUsersData.Contains(e.Username));

        return new CustomWebResponse
        {
            ResponseBody = new DemographicStatsDto
            {
                Gender = [.. filteredUsersData
                    .GroupBy(e => e.Gender)
                    .Where(group => !string.IsNullOrEmpty(group.Key))
                    .Select(group => new KeyValuePair<string, int>(group.Key, group.Count()))],
                Organization = [.. filteredUsersData
                    .GroupBy(e => e.Organization)
                    .Where(group => !string.IsNullOrEmpty(group.Key))
                    .Select(group => new KeyValuePair<string, int>(group.Key, group.Count()))],
                SelfRecognition = [.. filteredUsersData
                    .GroupBy(e => e.SelfRecognition)
                    .Where(group => !string.IsNullOrEmpty(group.Key))
                    .Select(group => new KeyValuePair<string, int>(group.Key, group.Count()))],
            },
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetIndicatorsStats(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default) =>
        new CustomWebResponse
        {
            ResponseBody = new IndicatorsStatsDto
            {
                IndicatorsByScale = await generalStatsRepository.GetIndicatorsByScaleAsync(departmentId, initiativeId, ct),
            },
        };
}
