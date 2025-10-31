namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using NetTopologySuite.Geometries;

/// <summary>
/// Custom Initiative repository interface.
/// </summary>
public interface IInitiativeRepository : IRepository<Initiative, int>
{
    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<Initiative> IncludeOdataEntities(IQueryable<Initiative> query);

    /// <summary>
    /// Get polygon centroid.
    /// </summary>
    /// <param name="locationIds">Location identifiers.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Polygon centroid.</returns>
    public Task<Point> GetCentroidAsync(int[] locationIds, CancellationToken ct = default);

    /// <summary>
    /// Get count of active initiatives.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of active initiatives.</returns>
    public Task<int> GetActiveInitiativesCountAsync(CancellationToken ct = default);

    /// <summary>
    /// Get total area of active initiatives with area.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers.</returns>
    public Task<double> GetTotalAreaOfActiveInitiativesAsync(CancellationToken ct = default);

    /// <summary>
    /// Get count of people involved in active initiatives.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of people involved in active initiatives.</returns>
    public Task<int> GetPeopleInvolvedInActiveInitiativesCountAsync(CancellationToken ct = default);
}
