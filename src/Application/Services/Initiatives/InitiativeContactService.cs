namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

/// <summary>
/// Initiative Contact service.
/// </summary>
public class InitiativeContactService : ServiceRead<InitiativeContact, InitiativeContactDto, int, InitiativeContactSpec>, IInitiativeContactService
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    public InitiativeContactService(
        IRepository<InitiativeContact> entityRepository,
        IMapper<InitiativeContact, InitiativeContactDto> mapper)
        : base(entityRepository, mapper)
    {
    }

    /// <summary>
    /// Get contacts by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> GetByInitiative(int initiativeId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.ListAsync(InitiativeContactSpec.InitiativeIdSpec(initiativeId), ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task<CustomWebResponse> Add(InitiativeContactDto entityData, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task<CustomWebResponse> Update(int id, InitiativeContactDto entityData, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task<CustomWebResponse> Delete(int id, CancellationToken ct = default) => throw new System.NotImplementedException();
}
