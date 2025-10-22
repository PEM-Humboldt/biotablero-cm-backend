namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using NetTopologySuite.Geometries;

/// <summary>
/// Custom Initiative repository interface.
/// </summary>
public interface IInitiativeRepository : IRepository<Initiative>
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

    /// <summary>
    /// Get count of active initiatives by department.
    /// </summary>
    /// <param name="departmentId">Department identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of active initiatives in the department.</returns>
    public Task<int> GetActiveInitiativesCountByDepartmentAsync(int departmentId, CancellationToken ct = default);

    /// <summary>
    /// Get total area of active initiatives by department.
    /// </summary>
    /// <param name="departmentId">Department identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers for the department.</returns>
    public Task<double> GetTotalAreaOfActiveInitiativesByDepartmentAsync(int departmentId, CancellationToken ct = default);

    /// <summary>
    /// Get count of people involved in active initiatives by department.
    /// </summary>
    /// <param name="departmentId">Department identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of people involved in active initiatives in the department.</returns>
    public Task<int> GetPeopleInvolvedInActiveInitiativesCountByDepartmentAsync(int departmentId, CancellationToken ct = default);

    /// <summary>
    /// Get count of active initiatives by specific initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of active initiatives (should be 1 or 0).</returns>
    public Task<int> GetActiveInitiativesCountByInitiativeAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get total area of active initiatives by specific initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers for the initiative.</returns>
    public Task<double> GetTotalAreaOfActiveInitiativesByInitiativeAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get count of people involved in active initiatives by specific initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of people involved in the specific initiative.</returns>
    public Task<int> GetPeopleInvolvedInActiveInitiativesCountByInitiativeAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get active initiatives with coordinates by location.
    /// </summary>
    /// <param name="locationId">Location identifier (optional). If null, returns all active initiatives.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of initiatives with coordinates.</returns>
    public Task<IEnumerable<(int Id, string Name, double[] Coordinate)>> GetActiveInitiativesWithCoordinatesByLocationAsync(int? locationId = null, CancellationToken ct = default);
}
