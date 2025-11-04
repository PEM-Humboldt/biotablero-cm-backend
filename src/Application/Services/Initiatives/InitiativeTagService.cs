namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Initiative Tag service.
/// </summary>
public class InitiativeTagService : IInitiativeTagService
{
    private readonly IInitiativeTagRepository entityRepository;
    private readonly ILogger logger;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly ITagRepository tagRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="tagRepository">Initiative Tag repository.</param>
    public InitiativeTagService(
        IInitiativeTagRepository entityRepository,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        ITagRepository tagRepository)
    {
        this.entityRepository = entityRepository;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
        this.tagRepository = tagRepository;
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="tagId">Initiative tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> AddAsync(int initiativeId, int tagId, CancellationToken ct = default)
    {
        // Validate initiative
        var initiativeExists = await initiativeRepository.ExistsByIdAsync(initiativeId, ct);

        if (!initiativeExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative not found",
            };
        }

        // Validate initiative tag
        var tagExists = await tagRepository.ExistsByIdAsync(tagId, ct);

        if (!tagExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Tag not found",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(initiativeId, tagId, ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "Duplicated initiative tag relationship",
            };
        }

        // Build entity data
        var entity = new InitiativeTag()
        {
            InitiativeId = initiativeId,
            TagId = tagId,
        };

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        logger.AddLog(LogType.Create, "Added initiative tag relationship", "{@EntityData}", entity);

        return new CustomWebResponse();
    }

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> DeleteAsync(int id, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = MessageConstants.NotFound,
            };
        }

        await entityRepository.DeleteAsync(entity, ct);

        logger.AddLog(LogType.Delete, "Deleted initiative tag relationship", "{@EntityData}", entity);

        return new CustomWebResponse();
    }
}
