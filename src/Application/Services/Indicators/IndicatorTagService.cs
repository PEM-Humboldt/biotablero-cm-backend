namespace IAVH.BioTablero.CM.Application.Services.Indicators;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Tags;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Indicator Tag service.
/// </summary>
public class IndicatorTagService : IIndicatorTagService
{
    private readonly IIndicatorTagRepository entityRepository;
    private readonly IValidationErrorTranslator errorTranslator;
    private readonly ILogger logger;
    private readonly IMapperRead<IndicatorTag, IndicatorTagDto> mapper;
    private readonly IIndicatorRepository indicatorRepository;
    private readonly ITagRepository tagRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="indicatorRepository">Indicator repository.</param>
    /// <param name="tagRepository">Tag repository.</param>
    public IndicatorTagService(
        IIndicatorTagRepository entityRepository,
        IMapperRead<IndicatorTag, IndicatorTagDto> mapper,
        IValidationErrorTranslator errorTranslator,
        ILogger logger,
        IIndicatorRepository indicatorRepository,
        ITagRepository tagRepository)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.errorTranslator = errorTranslator;
        this.logger = logger;
        this.indicatorRepository = indicatorRepository;
        this.tagRepository = tagRepository;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(int indicatorId, int tagId, CancellationToken ct = default)
    {
        // Validate indicator
        var indicator = await indicatorRepository.GetByIdAsync(indicatorId, ct);

        if (indicator == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.NotFound),
            };
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
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(indicatorId, tagId, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.Duplicated),
            };
        }

        // Build entity data
        var entity = new IndicatorTag()
        {
            IndicatorId = indicatorId,
            TagId = tagId,
        };

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);
        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added indicator tag relationship", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> DeleteAsync(int id, CancellationToken ct = default)
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

        await entityRepository.DeleteAsync(entity, ct);
        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted indicator tag relationship", "{@EntityData}", entityData);

        return new();
    }
}
