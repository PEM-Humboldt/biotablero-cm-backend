namespace IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

using System.Collections.Generic;
using System.Text;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums;

using IndicatorTypes = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums.IndicatorType;

/// <summary>
/// Constants for indicators.
/// </summary>
public static class IndicatorConstants
{
    /// <summary>
    /// Indicator date format.
    /// </summary>
    public static readonly CompositeFormat IndicatorDateFormat = CompositeFormat.Parse("{0}-{1}-01");

    /// <summary>
    /// Unit measures by indicator type.
    /// </summary>
    public static readonly Dictionary<IndicatorTypes, IndicatorMeasureUnit[]> UnitMeasuresByIndicatorType = new()
    {
        { IndicatorTypes.OccupiedAreaPercent, [IndicatorMeasureUnit.OccupiedAreaPercent] },
        { IndicatorTypes.DetectionOccupancyProbability, [IndicatorMeasureUnit.DetectionProbability, IndicatorMeasureUnit.OccupancyProbability] },
        { IndicatorTypes.SpeciesDiversity, [IndicatorMeasureUnit.SpeciesRichness, IndicatorMeasureUnit.ShannonIndex, IndicatorMeasureUnit.SimpsonIndex] },
        { IndicatorTypes.RelativeUseByBiologicalGroup, [IndicatorMeasureUnit.RelativeUseIndex] },
        { IndicatorTypes.CentralRelationalIntensity, [IndicatorMeasureUnit.RelationalIntensity] },
        { IndicatorTypes.CollectiveActionParticipation, [IndicatorMeasureUnit.PersonCount] },
    };

    /// <summary>
    /// Indicators without group required.
    /// </summary>
    public static readonly IndicatorTypes[] IndicatorsWithoutGroupRequired =
    [
        IndicatorTypes.SpeciesDiversity,
    ];

    /// <summary>
    /// Indicators with species.
    /// </summary>
    public static readonly IndicatorTypes[] IndicatorsWithSpecies =
    [
            IndicatorTypes.OccupiedAreaPercent,
            IndicatorTypes.DetectionOccupancyProbability,
            IndicatorTypes.RelativeUseByBiologicalGroup,
    ];

    /// <summary>
    /// Indicators with confidence interval.
    /// </summary>
    public static readonly IndicatorTypes[] IndicatorsWithConfidenceInterval =
    [
            IndicatorTypes.DetectionOccupancyProbability,
            IndicatorTypes.SpeciesDiversity,
    ];

    /// <summary>
    /// Indicators with date range.
    /// </summary>
    public static readonly IndicatorTypes[] IndicatorsWithDateRange =
    [
            IndicatorTypes.RelativeUseByBiologicalGroup,
            IndicatorTypes.CollectiveActionParticipation,
    ];
}
