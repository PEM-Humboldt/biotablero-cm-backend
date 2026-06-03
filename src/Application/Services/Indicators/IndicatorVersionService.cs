namespace IAVH.BioTablero.CM.Application.Services.Indicators;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

/// <summary>
/// Indicator Version service.
/// </summary>
public class IndicatorVersionService : ServiceRead<IndicatorVersion, IndicatorVersionDto, int>, IIndicatorVersionService
{
    private new readonly IIndicatorVersionRepository entityRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    public IndicatorVersionService(
        IIndicatorVersionRepository entityRepository,
        IMapperRead<IndicatorVersion, IndicatorVersionDto> mapper,
        IValidationErrorTranslator errorTranslator)
    : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
    }
}
