namespace IAVH.BioTablero.CM.Application.Services.Reports;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Iam;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Reports;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;
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
    public async Task<CustomWebResponse> GetGeneralStatsAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default) =>
        new()
        {
            ResponseBody = new GeneralStatsDto
            {
                EnabledInitiatives = await generalStatsRepository.GetEnabledRecordsCountAsync(departmentId: departmentId, initiativeId: initiativeId, ct: ct),
                PeopleInvolved = await generalStatsRepository.GetPeopleInvolvedCountAsync(departmentId, initiativeId, ct),
                AgreementsInvolved = await generalStatsRepository.GetAgreementsInvolvedCountAsync(departmentId, initiativeId, ct),
                Area = await generalStatsRepository.GetAreaAsync(departmentId, initiativeId, ct),
            },
        };

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
        var internalUsersData = await generalStatsRepository.GetUserNamesAsync(departmentId, initiativeId, ct);
        var externalUsersData = await iamService.GetUsersDataAsync([.. internalUsersData], ct);
        DemographicStatsDto responseBody = externalUsersData == null || !externalUsersData.Any() ? new() : new()
        {
            Gender = ProcessUserGroup(externalUsersData.GroupBy(e => e.Gender!)),
            Organization = ProcessUserGroup(externalUsersData.GroupBy(e => e.Organization!)),
            SelfRecognition = ProcessUserGroup(externalUsersData.GroupBy(e => e.SelfRecognition!)),
        };

        return new CustomWebResponse
        {
            ResponseBody = responseBody,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetIndicatorsStats(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default) =>
        new()
        {
            ResponseBody = new IndicatorsStatsDto
            {
                IndicatorsByScale = await generalStatsRepository.GetIndicatorsByScaleAsync(departmentId, initiativeId, ct),
            },
        };

    /// <summary>
    /// Process user group data.
    /// </summary>
    /// <param name="group">External users group.</param>
    /// <returns>Grouped users data.</returns>
    private static List<KeyValuePair<string, int>> ProcessUserGroup(IEnumerable<IGrouping<string, ExternalUser>> group)
    {
        if (group == null || !group.Any())
        {
            return [];
        }

        return [.. group
            .Where(group => !string.IsNullOrEmpty(group.Key))
            .Select(group => new KeyValuePair<string, int>(group.Key, group.Count()))];
    }
}
