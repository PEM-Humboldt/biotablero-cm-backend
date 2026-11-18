namespace IAVH.BioTablero.CM.Application.Services.Reports;

using System;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Reports;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Reports;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Report Data service.
/// </summary>
public class ReportDataService : ServiceRead<ReportData, ReportDataDto, int>, IReportDataService
{
    private readonly IValidator<ReportDataDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateAndRead<ReportData, ReportDataDto> mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    public ReportDataService(
        IRepository<ReportData, int> entityRepository,
        IMapperCreateAndRead<ReportData, ReportDataDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<ReportDataDto> entityValidator,
        ILogger logger)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(ReportDataDto entityData, CancellationToken ct = default)
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

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.CreationDate = DateTimeOffset.UtcNow;

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added report data", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }
}
