namespace IAVH.BioTablero.CM.Application.Services.Indicators;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

/// <summary>
/// Indicator service.
/// </summary>
public class IndicatorService : ServiceRead<Indicator, IndicatorDto, int>, IIndicatorService
{
    private new readonly IMapperRead<Indicator, IndicatorDto> mapper;
    private new readonly IIndicatorRepository entityRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    public IndicatorService(
        IIndicatorRepository entityRepository,
        IMapperRead<Indicator, IndicatorDto> mapper,
        IValidationErrorTranslator errorTranslator)
    : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
    }
}
