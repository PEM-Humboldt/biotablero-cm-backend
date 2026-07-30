namespace IAVH.BioTablero.CM.Application.Services.Indicators;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Spreadsheets.Services;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using Microsoft.AspNetCore.OData.Query;

using IndicatorMeasureUnits = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums.IndicatorMeasureUnit;
using IndicatorTypes = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums.IndicatorType;

/// <summary>
/// Indicator service.
/// </summary>
public class IndicatorService : ServiceRead<Indicator, IndicatorDto, int>, IIndicatorService
{
    private new readonly IIndicatorRepository entityRepository;
    private readonly IIndicatorExcelService excelService;
    private readonly ILocationRepository locationRepository;
    private readonly IIndicatorVersionRepository indicatorVersionRepository;
    private readonly ICategoryRepository categoryRepository;
    private readonly IValidator<IndicatorsImportRow> indicatorsImportRowValidator;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="excelService">Excel service.</param>
    /// <param name="locationRepository">Location repository.</param>
    /// <param name="indicatorVersionRepository">Indicator version repository.</param>
    /// <param name="categoryRepository">Indicator Category repository.</param>
    /// <param name="indicatorsImportRowValidator">Indicators spreadsheet row validator.</param>
    public IndicatorService(
        IIndicatorRepository entityRepository,
        IMapperRead<Indicator, IndicatorDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IIndicatorExcelService excelService,
        ILocationRepository locationRepository,
        IIndicatorVersionRepository indicatorVersionRepository,
        ICategoryRepository categoryRepository,
        IValidator<IndicatorsImportRow> indicatorsImportRowValidator)
    : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.excelService = excelService;
        this.locationRepository = locationRepository;
        this.indicatorVersionRepository = indicatorVersionRepository;
        this.categoryRepository = categoryRepository;
        this.indicatorsImportRowValidator = indicatorsImportRowValidator;
    }

    /// <inheritdoc/>
    public override async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<Indicator> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.IncludeOdataEntities(query);

        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.GetByInitiativeAsync(initiativeId, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> ImportIndicatorsAsync(string userName, IndicatorsImportFileDto requestData, IInputFile formFile, CancellationToken ct)
    {
        var fileReadResult = excelService.GetFileData(formFile);

        if (fileReadResult.Errors.Count > 0)
        {
            return new(true)
            {
                ResponseBody = fileReadResult.Errors,
            };
        }

        Indicator indicator = null;

        if (requestData.Id.HasValue)
        {
            indicator = await entityRepository.GetByIdAsync(requestData.Id.Value, ct);

            if (indicator == null)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.NotFound),
                };
            }
        }

        // Validate data
        foreach (var row in fileReadResult.Rows)
        {
            var validationResult = await indicatorsImportRowValidator.ValidateAsync(row, ct);

            if (!validationResult.IsValid)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(validationResult.Errors),
                    Message = $"Row: {row.RowNumber + 1}",
                };
            }
        }

        // Validate upper groups
        var upperGroups = fileReadResult.Rows
            .Select(r => r.UpperGroupName)
            .Distinct()
            .ToArray();

        var upperGroupEntities = await categoryRepository.GetUpperGroupsAsync(upperGroups, ct);

        if (upperGroups.Length != upperGroupEntities.Count)
        {
            var upperGroupEntitiesStr = upperGroupEntities
                .Select(e => e.Name);

            foreach (var upperGroup in upperGroups)
            {
                if (!upperGroupEntitiesStr.Contains(upperGroup))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.UpperGroupNotFound, data: $"{upperGroup}"),
                    };
                }
            }
        }

        // Validate general data
        var indicatorsWithoutGroupRequired = new IndicatorTypes[]
        {
            IndicatorTypes.SpeciesDiversity,
            IndicatorTypes.CentralRelationalIntensity,
            IndicatorTypes.CollectiveActionParticipation,
        };

        var indicatorsWithSpecies = new IndicatorTypes[]
        {
            IndicatorTypes.OccupiedAreaPercent,
            IndicatorTypes.DetectionOccupancyProbability,
            IndicatorTypes.RelativeUseByBiologicalGroup,
        };

        var indicatorsWithConfidenceInterval = new IndicatorTypes[]
        {
            IndicatorTypes.DetectionOccupancyProbability,
            IndicatorTypes.SpeciesDiversity,
        };

        var groupedGroups = fileReadResult.Rows
            .GroupBy(e => new { e.GroupName, e.GroupDescription });

        foreach (var row in fileReadResult.Rows)
        {
            if (!Enum.GetValues<IndicatorTypes>().Select(e => (int)e).Contains(row.IndicatorTypeId))
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidIndicatorType),
                };
            }

            foreach (var measureUnit in IndicatorConstants.UnitMeasuresByIndicatorType)
            {
                if (row.IndicatorTypeId == (int)measureUnit.Key && !measureUnit.Value.Contains((IndicatorMeasureUnits)row.MeasureUnitId))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidMeasureUnit),
                    };
                }
            }

            if (indicatorsWithSpecies.Contains((IndicatorTypes)row.IndicatorTypeId))
            {
                if (string.IsNullOrEmpty(row.GroupName) || string.IsNullOrEmpty(row.GroupDescription))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.GroupAndDescriptionRequired),
                    };
                }
            }

            if (indicatorsWithoutGroupRequired.Contains((IndicatorTypes)row.IndicatorTypeId))
            {
                if (!string.IsNullOrEmpty(row.GroupName) || !string.IsNullOrEmpty(row.GroupDescription))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.GroupAndDescriptionNotRequired, data: $"{row.GroupName}, {row.GroupDescription}"),
                    };
                }
            }

            if (indicatorsWithConfidenceInterval.Contains((IndicatorTypes)row.IndicatorTypeId))
            {
                if (row.UpperLimit == null || row.LowerLimit == null)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.ConfidenceIntervalRequired),
                    };
                }
            }

            if (row.UpperLimit.HasValue && row.LowerLimit.HasValue && (row.UpperLimit < row.Value || row.LowerLimit > row.Value))
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidConfidenceInterval),
                };
            }
        }

        // Get locations data
        var locationEntities = fileReadResult.Rows
            .GroupBy(r => new { r.DepartmentName, r.MunicipalityName })
            .Select(async g => await locationRepository.GetByDepartmentAndMunicipalityNamesAsync(g.Key.DepartmentName, g.Key.MunicipalityName, ct))
            .Select(r => r.Result)
            .Where(e => e != null);

        var totalIndicators = fileReadResult.Rows
            .GroupBy(r => r.IndicatorTypeId)
            .Count();

        if (indicator != null && totalIndicators != 1)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.OnlyOneIndicatorRequired),
            };
        }

        // var now = DateTimeOffset.Now;

        // var indicators = fileReadResult.Rows
        //     .GroupBy(r => r.IndicatorTypeId)
        //     .Select(async g => new Indicator()
        //     {
        //         InitiativeId = requestData.InitiativeId,
        //         Name = $"{g.Select(r => r.IndicatorTypeId).FirstOrDefault()} ({now.ToFileTime})",
        //         Type = new() { Id = g.Key },
        //         IndicatorLocations = [.. g.Select(r => new IndicatorLocation()
        //         {
        //             LocationId = locationEntities.FirstOrDefault(l => l.Name == r.MunicipalityName && l.Parent.Name == r.DepartmentName)?.Id ?? 0,
        //             Locality = r.LocalityName,
        //         })],
        //         Versions = [
        //             new()
        //             {
        //                 CreationDate = now,
        //                 Version = indicator == null ? 1 : await indicatorVersionRepository.GetLastVersion(indicator.Id, ct),
        //                 Groups = [
        //                     new()
        //                     {
        //                         CategoryId =
        //                     }
        //                 ],
        //             },
        //         ],
        //     });
        return new();
    }
}
