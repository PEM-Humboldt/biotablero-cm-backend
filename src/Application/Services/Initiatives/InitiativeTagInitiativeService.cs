namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Initiative Tag Initiative service.
/// </summary>
public class InitiativeTagInitiativeService : IInitiativeTagInitiativeService
{
    private readonly IRepository<InitiativeTagInitiative> entityRepository;
    private readonly ILogger logger;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly IRepository<InitiativeTag> initiativeTagRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="initiativeTagRepository">Initiative Tag repository.</param>
    public InitiativeTagInitiativeService(
        IRepository<InitiativeTagInitiative> entityRepository,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        IRepository<InitiativeTag> initiativeTagRepository)
    {
        this.entityRepository = entityRepository;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
        this.initiativeTagRepository = initiativeTagRepository;
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="tagId">Initiative tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Add(int initiativeId, int tagId, CancellationToken ct = default)
    {
        // Validate initiative
        var initiativeExists = await initiativeRepository.AnyAsync(new InitiativeSpec(initiativeId), ct);

        if (!initiativeExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative not found",
            };
        }

        // Validate initiative tag
        var initiativeTagExists = await initiativeTagRepository.AnyAsync(new InitiativeTagSpec(tagId), ct);

        if (!initiativeTagExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative tag not found",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.AnyAsync(InitiativeTagInitiativeSpec.GetDuplicatesSpec(initiativeId, tagId), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "Duplicated initiative tag relationship",
            };
        }

        // Build entity data
        var entity = new InitiativeTagInitiative()
        {
            InitiativeId = initiativeId,
            TagId = tagId,
        };

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        logger.AddLog(LogType.Create, "Added initiative tag relationship: {@entityData}", entity);

        return new CustomWebResponse();
    }

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Delete(int id, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        await entityRepository.DeleteAsync(entity, ct);

        logger.AddLog(LogType.Delete, "Deleted initiative tag relationship: {@entityData}", entity);

        return new CustomWebResponse();
    }
}
