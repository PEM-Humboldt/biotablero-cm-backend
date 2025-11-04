namespace IAVH.BioTablero.CM.Application.Services.Statistics;

using System;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
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

    /// <summary>
    /// Get general statistics for community monitoring.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data.</returns>
    public async Task<CustomWebResponse> GetGeneralStatisticsAsync(CancellationToken ct = default)
    {
        // Calculate statistics using private methods
        var totalActiveInitiatives = await GetTotalActiveInitiativesCountAsync(ct);
        var totalPeopleInvolved = await GetTotalPeopleInvolvedCountAsync(ct);
        var totalAreaInHectares = await GetTotalAreaInHectaresAsync(ct);

        // Validate data integrity
        if (totalActiveInitiatives < 0)
        {
            return new CustomWebResponse(true)
            {
                Message = "Invalid data: negative initiative count",
            };
        }

        if (totalPeopleInvolved < 0)
        {
            return new CustomWebResponse(true)
            {
                Message = "Invalid data: negative people count",
            };
        }

        if (totalAreaInHectares < 0)
        {
            return new CustomWebResponse(true)
            {
                Message = "Invalid data: negative area value",
            };
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

    /// <summary>
    /// Get total count of active initiatives.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of active initiatives.</returns>
    private async Task<int> GetTotalActiveInitiativesCountAsync(CancellationToken ct = default) =>
        await initiativeRepository.GetActiveInitiativesCountAsync(ct);

    /// <summary>
    /// Get total count of people involved in active initiatives.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of people involved.</returns>
    private async Task<int> GetTotalPeopleInvolvedCountAsync(CancellationToken ct = default) =>
        await initiativeRepository.GetPeopleInvolvedInActiveInitiativesCountAsync(ct);

    /// <summary>
    /// Get total area of active initiatives in hectares.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in hectares.</returns>
    private async Task<double> GetTotalAreaInHectaresAsync(CancellationToken ct = default)
    {
        var totalAreaInSquareKm = await initiativeRepository.GetTotalAreaOfActiveInitiativesAsync(ct);

        // Convert from km² to hectares (1 km² = 100 hectares)
        return Math.Round(totalAreaInSquareKm * 100, 2);
    }
}
