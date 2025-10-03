namespace IAVH.BioTablero.CM.Application.Services;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// General statistics service implementation.
/// </summary>
public class GeneralStatisticsService : IGeneralStatisticsService
{
    private readonly IRepository<Initiative> initiativeRepository;
    private readonly IRepository<InitiativeUser> initiativeUserRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="initiativeUserRepository">Initiative user repository.</param>
    public GeneralStatisticsService(
        IRepository<Initiative> initiativeRepository,
        IRepository<InitiativeUser> initiativeUserRepository)
    {
        this.initiativeRepository = initiativeRepository;
        this.initiativeUserRepository = initiativeUserRepository;
    }

    /// <summary>
    /// Get general statistics for community monitoring.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data.</returns>
    public async Task<CustomWebResponse> GetGeneralStatisticsAsync(CancellationToken ct = default)
    {
        try
        {
            var statistics = new GeneralStatisticsDto();

            // Get active initiatives query
            var activeInitiativesQuery = initiativeRepository.GetQueryable().Where(i => i.Enabled);

            // 1. Número total de iniciativas activas en la plataforma
            var activeInitiatives = await initiativeRepository.QueryToListAsync(activeInitiativesQuery, ct);
            statistics.TotalActiveInitiatives = activeInitiatives.Count;

            // 2. Número de personas involucradas en iniciativas activas
            var activeInitiativeIds = activeInitiatives.Select(i => i.Id).ToList();
            var peopleInActiveInitiativesQuery = initiativeUserRepository.GetQueryable()
                .Where(iu => activeInitiativeIds.Contains(iu.InitiativeId));
            var peopleInActiveInitiatives = await initiativeUserRepository.QueryToListAsync(peopleInActiveInitiativesQuery, ct);
            statistics.TotalPeopleInvolved = peopleInActiveInitiatives.Count;

            // 3. Área total de las iniciativas activas en hectáreas
            var totalAreaQuery = activeInitiativesQuery.Where(i => i.PolygonArea > 0);
            var initiativesWithArea = await initiativeRepository.QueryToListAsync(totalAreaQuery, ct);
            var totalAreaInSquareKm = initiativesWithArea.Sum(i => i.PolygonArea);

            // Convertir de km² a hectáreas (1 km² = 100 hectáreas)
            statistics.TotalAreaInHectares = Math.Round(totalAreaInSquareKm * 100, 2);

            return new CustomWebResponse
            {
                ResponseBody = statistics,
            };
        }
        catch (Exception ex)
        {
            return new CustomWebResponse(true)
            {
                Message = $"Error al obtener las estadísticas generales: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
            };
        }
    }
}
