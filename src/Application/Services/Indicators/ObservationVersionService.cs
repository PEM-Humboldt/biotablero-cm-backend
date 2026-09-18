namespace IAVH.BioTablero.CM.Application.Services.Indicators;

using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Observation Version service.
/// </summary>
public class ObservationVersionService : ServiceRead<ObservationVersion, ObservationVersionDto, int>, IObservationVersionService
{
    private readonly ILogger logger;
    private new readonly IMapperReadAndUpdate<ObservationVersion, ObservationVersionDto> mapper;
    private readonly IValidator<ObservationVersionDto> entityValidator;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    public ObservationVersionService(
        IObservationVersionRepository entityRepository,
        IValidator<ObservationVersionDto> entityValidator,
        ILogger logger,
        IMapperReadAndUpdate<ObservationVersion, ObservationVersionDto> mapper,
        IValidationErrorTranslator errorTranslator)
    : base(entityRepository, mapper, errorTranslator)
    {
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, ObservationVersionDto entityData, CancellationToken ct = default)
    {
        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, ct);

        if (!validationResult.IsValid)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(validationResult.Errors),
            };
        }

        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        // Update entity data
        mapper.Update(entity, entityData);

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated observation version", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }
}
