namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Initiatives;

using NetTopologySuite.Geometries;

/// <summary>
/// Initiative repository interface.
/// </summary>
public interface IInitiativeRepository : IRepository<Initiative, int>
{
    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    IQueryable<Initiative> IncludeOdataEntities(IQueryable<Initiative> query);

    /// <summary>
    /// Get elements by user name.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Elements list.</returns>
    Task<IEnumerable<Initiative>> GetByUserNameAsync(string userName, CancellationToken ct = default);

    /// <summary>
    /// Check authorized entity modification.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if the modification is authorized. False otherwise.</returns>
    Task<bool> AuthorizedEntityModifyAsync(int id, string userName, CancellationToken ct = default);

    /// <summary>
    /// Get if elements exists by name.
    /// </summary>
    /// <param name="name">Entity name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> AnyByNameAsync(string name, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="name">Entity name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int id, string name, CancellationToken ct = default);

    /// <summary>
    /// Check if elements exists by tag.
    /// </summary>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> AnyByTagAsync(int tagId, CancellationToken ct = default);

    /// <summary>
    /// Get polygon centroid.
    /// </summary>
    /// <param name="locationIds">Location identifiers.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Polygon centroid.</returns>
    Task<Point> GetCentroidAsync(int[] locationIds, CancellationToken ct = default);

    /// <summary>
    /// Get count of active initiatives.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of active initiatives.</returns>
    Task<int> GetActiveInitiativesCountAsync(CancellationToken ct = default);

    /// <summary>
    /// Get total area of active initiatives with area.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers.</returns>
    Task<double> GetTotalAreaOfActiveInitiativesAsync(CancellationToken ct = default);

    /// <summary>
    /// Get count of people involved in active initiatives.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of people involved in active initiatives.</returns>
    Task<int> GetPeopleInvolvedInActiveInitiativesCountAsync(CancellationToken ct = default);

    /// <summary>
    /// Get count of active initiatives by department.
    /// </summary>
    /// <param name="departmentId">Department identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of active initiatives in the department.</returns>
    Task<int> GetActiveInitiativesCountByDepartmentAsync(int departmentId, CancellationToken ct = default);

    /// <summary>
    /// Get total area of active initiatives by department.
    /// </summary>
    /// <param name="departmentId">Department identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers for the department.</returns>
    Task<double> GetTotalAreaOfActiveInitiativesByDepartmentAsync(int departmentId, CancellationToken ct = default);

    /// <summary>
    /// Get count of people involved in active initiatives by department.
    /// </summary>
    /// <param name="departmentId">Department identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of people involved in active initiatives in the department.</returns>
    Task<int> GetPeopleInvolvedInActiveInitiativesCountByDepartmentAsync(int departmentId, CancellationToken ct = default);

    /// <summary>
    /// Get count of active initiatives by specific initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of active initiatives (should be 1 or 0).</returns>
    Task<int> GetActiveInitiativesCountByInitiativeAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get total area of active initiatives by specific initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers for the initiative.</returns>
    Task<double> GetTotalAreaOfActiveInitiativesByInitiativeAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get count of people involved in active initiatives by specific initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of people involved in the specific initiative.</returns>
    Task<int> GetPeopleInvolvedInActiveInitiativesCountByInitiativeAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get active initiatives with coordinates by location.
    /// </summary>
    /// <param name="locationId">Location identifier (optional). If null, returns all active initiatives.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of initiatives with coordinates.</returns>
    Task<IEnumerable<InitiativeGeoData>> GetActiveInitiativesWithCoordinatesByLocationAsync(int? locationId = null, CancellationToken ct = default);

    /// <summary>
    /// Get last created entities.
    /// </summary>
    /// <param name="count">Number of entities.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of last created initiatives.</returns>
    Task<IEnumerable<Initiative>> GetLastEntitiesAsync(int count, CancellationToken ct = default);
}
