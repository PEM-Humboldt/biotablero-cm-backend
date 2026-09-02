namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Reports;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Monitoring Events service.
/// </summary>
public class MonitoringEventsService : ServiceRead<MonitoringEvents, MonitoringEventsDto, int>, IMonitoringEventsService
{
    private new readonly IMonitoringEventsRepository entityRepository;
    private readonly IValidator<MonitoringEventsDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateReadAndUpdate<MonitoringEvents, MonitoringEventsDto> mapper;
    private readonly IInitiativeRepository initiativeRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    public MonitoringEventsService(
        IMonitoringEventsRepository entityRepository,
        IMapperCreateReadAndUpdate<MonitoringEvents, MonitoringEventsDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<MonitoringEventsDto> entityValidator,
        ILogger logger,
        IInitiativeRepository initiativeRepository)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetListAsync(int initiativeId, ODataQueryOptions<MonitoringEvents> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.AddInitiativeFilter(initiativeId, query);
        query = entityRepository.IncludeOdataEntities(query);

        try
        {
            var odataResponse = await GetOdataDtoListByQueryAsync(query, queryOptions, ct);
            return GetOdataWebResponse(odataResponse, mapper);
        }
        catch (ODataException)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.OdataInvalidFilter),
            };
        }
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(MonitoringEventsDto entityData, CancellationToken ct = default)
    {
        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, options => options.IncludeRuleSets("default", "Create"), ct);

        if (!validationResult.IsValid)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(validationResult.Errors),
            };
        }

        // Validate initiative
        var initiativeId = entityData.InitiativeId ?? 0;
        var initiative = await initiativeRepository.GetByIdAsync(initiativeId, ct);

        if (initiative == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.NotFound),
            };
        }

        if (!initiative.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Disabled),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(initiativeId, entityData.Date, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.MonitoringEvents.Duplicated),
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added monitoring events", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, MonitoringEventsDto entityData, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);
        var initiativeId = entity?.InitiativeId ?? 0;

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        // Validate initiative
        var initiative = await initiativeRepository.GetByIdAsync(initiativeId, ct);

        if (!(initiative?.Enabled ?? false))
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Disabled),
            };
        }

        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, ct);

        if (!validationResult.IsValid)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(validationResult.Errors),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, entity.InitiativeId, entityData.Date, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.MonitoringEvents.Duplicated),
            };
        }

        // Update entity data
        mapper.Update(entity, entityData);

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated monitoring events", "{@EntityData}", entityData);

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
        var initiativeId = entity?.InitiativeId ?? 0;

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        // Validate initiative
        var initiative = await initiativeRepository.GetByIdAsync(initiativeId, ct);

        if (!(initiative?.Enabled ?? false))
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Disabled),
            };
        }

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted monitoring events", "{@EntityData}", entityData);

        return new();
    }
}
