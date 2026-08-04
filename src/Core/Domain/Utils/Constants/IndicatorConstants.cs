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
    /// "Total" group name.
    /// </summary>
    public const string TotalGroupName = "Total";

    /// <summary>
    /// "Species" category name.
    /// </summary>
    public const string SpeciesCategoryName = "Especie";

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

    /// <summary>
    /// Indicators with predefined categories.
    /// </summary>
    public static readonly IndicatorTypes[] IndicatorsWithPredefinedCategories =
    [
        IndicatorTypes.SpeciesDiversity,
        IndicatorTypes.CentralRelationalIntensity,
        IndicatorTypes.CollectiveActionParticipation,
    ];
}
