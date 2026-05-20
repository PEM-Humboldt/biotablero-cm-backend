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
public class GeneralStatisticsService : IGeneralStatisticsService
{
    private readonly IInitiativeRepository initiativeRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="initiativeRepository">Initiative repository.</param>
    public GeneralStatisticsService(IInitiativeRepository initiativeRepository)
    {
        this.initiativeRepository = initiativeRepository;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetGeneralStatisticsAsync(CancellationToken ct = default) =>
        await GetGeneralStatisticsInternalAsync(null, null, ct);

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetGeneralStatisticsByDepartmentAsync(int departmentId, CancellationToken ct = default) =>
        await GetGeneralStatisticsInternalAsync(departmentId, null, ct);

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetGeneralStatisticsByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await GetGeneralStatisticsInternalAsync(null, initiativeId, ct);

    /// <summary>
    /// Internal method to get general statistics with optional filters.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data.</returns>
    private async Task<CustomWebResponse> GetGeneralStatisticsInternalAsync(int? departmentId, int? initiativeId, CancellationToken ct = default)
    {
        // Calculate statistics based on filters
        int totalActiveInitiatives;
        int totalPeopleInvolved;
        double totalAreaInHectares;

        if (departmentId.HasValue)
        {
            totalActiveInitiatives = await initiativeRepository.GetActiveInitiativesCountByDepartmentAsync(departmentId.Value, ct);
            totalPeopleInvolved = await initiativeRepository.GetPeopleInvolvedInActiveInitiativesCountByDepartmentAsync(departmentId.Value, ct);
            var totalAreaInSquareKm = await initiativeRepository.GetTotalAreaOfActiveInitiativesByDepartmentAsync(departmentId.Value, ct);
            totalAreaInHectares = GeometryUtils.ConvertSquareKilometersToHectares(totalAreaInSquareKm);
        }
        else if (initiativeId.HasValue)
        {
            totalActiveInitiatives = await initiativeRepository.GetActiveInitiativesCountByInitiativeAsync(initiativeId.Value, ct);
            totalPeopleInvolved = await initiativeRepository.GetPeopleInvolvedInActiveInitiativesCountByInitiativeAsync(initiativeId.Value, ct);
            var totalAreaInSquareKm = await initiativeRepository.GetTotalAreaOfActiveInitiativesByInitiativeAsync(initiativeId.Value, ct);
            totalAreaInHectares = GeometryUtils.ConvertSquareKilometersToHectares(totalAreaInSquareKm);
        }
        else
        {
            totalActiveInitiatives = await initiativeRepository.GetEnabledRecordsCountAsync(ct);
            totalPeopleInvolved = await initiativeRepository.GetPeopleInvolvedInActiveInitiativesCountAsync(ct);
            var totalAreaInSquareKm = await initiativeRepository.GetTotalAreaOfActiveInitiativesAsync(ct);
            totalAreaInHectares = GeometryUtils.ConvertSquareKilometersToHectares(totalAreaInSquareKm);
        }

        var statistics = new GeneralStatisticsDto
        {
            TotalActiveInitiatives = totalActiveInitiatives,
            TotalPeopleInvolved = totalPeopleInvolved,
            TotalAreaInHectares = totalAreaInHectares,
        };

        return new CustomWebResponse
        {
            ResponseBody = statistics,
        };
    }
}
