namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System.Net;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Tags;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Initiative Tag service.
/// </summary>
public class InitiativeTagService : IInitiativeTagService
{
    private readonly IInitiativeTagRepository entityRepository;
    private readonly IValidationErrorTranslator errorTranslator;
    private readonly ILogger logger;
    private readonly IMapperRead<InitiativeTag, InitiativeTagDto> mapper;
    private readonly IInitiativeUserRepository initiativeUserRepository;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly ITagRepository tagRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeUserRepository">Initiative user repository.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="tagRepository">Tag repository.</param>
    public InitiativeTagService(
        IInitiativeTagRepository entityRepository,
        IMapperRead<InitiativeTag, InitiativeTagDto> mapper,
        IValidationErrorTranslator errorTranslator,
        ILogger logger,
        IInitiativeUserRepository initiativeUserRepository,
        IInitiativeRepository initiativeRepository,
        ITagRepository tagRepository)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.errorTranslator = errorTranslator;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
        this.tagRepository = tagRepository;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(string userName, bool userIsAdmin, int initiativeId, int tagId, CancellationToken ct = default)
    {
        // Validate initiative
        var initiativeExists = await initiativeRepository.AnyAsync(initiativeId, ct);

        if (!initiativeExists)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.NotFound),
            };
        }

        // Validate user permissions
        if (!userIsAdmin)
        {
            var authorizedUserAction = await initiativeUserRepository.AnyByInitiativeUserAndLevelAsync(initiativeId, userName, (int)InitiativeUserLevelEnum.Leader, ct);

            if (!authorizedUserAction)
            {
                return new(true)
                {
                    StatusCode = HttpStatusCode.Forbidden,
                };
            }
        }

        // Validate tag
        var tagExists = await tagRepository.AnyAsync(tagId, ct);

        if (!tagExists)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Tags.NotFound),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(initiativeId, tagId, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.Duplicated),
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
        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added initiative tag relationship", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> DeleteAsync(int id, string userName, bool userIsAdmin, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        // Validate user permissions
        if (!userIsAdmin)
        {
            var authorizedUserAction = await initiativeUserRepository.AnyByInitiativeUserAndLevelAsync(entity.InitiativeId, userName, (int)InitiativeUserLevelEnum.Leader, ct);

            if (!authorizedUserAction)
            {
                return new(true)
                {
                    StatusCode = HttpStatusCode.Forbidden,
                };
            }
        }

        await entityRepository.DeleteAsync(entity, ct);

        logger.AddLog(LogType.Delete, "Deleted initiative tag relationship", "{@EntityData}", entity);

        return new();
    }
}
