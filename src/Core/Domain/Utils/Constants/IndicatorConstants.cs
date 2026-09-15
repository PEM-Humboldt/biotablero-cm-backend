namespace IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

using System.Collections.Generic;
using System.Text;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums;

using IndicatorTopics = Enums.IndicatorsEnums.IndicatorTopic;

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
    /// Observation date format.
    /// </summary>
    public static readonly CompositeFormat ObservationDateFormat = CompositeFormat.Parse("{0}-{1}-01");

    /// <summary>
    /// Unit measures by indicator topic.
    /// </summary>
    public static readonly Dictionary<IndicatorTopics, IndicatorMeasureUnit[]> UnitMeasuresByIndicatorTopic = new()
    {
        { IndicatorTopics.OccupiedAreaPercent, [IndicatorMeasureUnit.OccupiedAreaPercent] },
        { IndicatorTopics.DetectionOccupancyProbability, [IndicatorMeasureUnit.DetectionProbability, IndicatorMeasureUnit.OccupancyProbability] },
        { IndicatorTopics.SpeciesDiversity, [IndicatorMeasureUnit.SpeciesRichness, IndicatorMeasureUnit.ShannonIndex, IndicatorMeasureUnit.SimpsonIndex] },
        { IndicatorTopics.RelativeUseByBiologicalGroup, [IndicatorMeasureUnit.RelativeUseIndex] },
        { IndicatorTopics.CentralRelationalIntensity, [IndicatorMeasureUnit.RelationalIntensity] },
        { IndicatorTopics.CollectiveActionParticipation, [IndicatorMeasureUnit.PersonCount] },
    };

    /// <summary>
    /// Indicators with species.
    /// </summary>
    public static readonly IndicatorTopics[] IndicatorsWithSpecies =
    [
        IndicatorTopics.OccupiedAreaPercent,
        IndicatorTopics.DetectionOccupancyProbability,
        IndicatorTopics.RelativeUseByBiologicalGroup,
    ];

    /// <summary>
    /// Indicators with confidence interval.
    /// </summary>
    public static readonly IndicatorTopics[] IndicatorsWithConfidenceInterval =
    [
        IndicatorTopics.DetectionOccupancyProbability,
        IndicatorTopics.SpeciesDiversity,
    ];

    /// <summary>
    /// Indicators with date range.
    /// </summary>
    public static readonly IndicatorTopics[] IndicatorsWithDateRange =
    [
        IndicatorTopics.RelativeUseByBiologicalGroup,
    ];

    /// <summary>
    /// Indicators with predefined categories.
    /// </summary>
    public static readonly IndicatorTopics[] IndicatorsWithPredefinedCategories =
    [
        IndicatorTopics.SpeciesDiversity,
        IndicatorTopics.CentralRelationalIntensity,
        IndicatorTopics.CollectiveActionParticipation,
    ];

    /// <summary>
    /// Indicators with integer values.
    /// </summary>
    public static readonly IndicatorTopics[] IndicatorsWithIntegerValues =
    [
        IndicatorTopics.RelativeUseByBiologicalGroup,
        IndicatorTopics.CollectiveActionParticipation,
    ];
}
