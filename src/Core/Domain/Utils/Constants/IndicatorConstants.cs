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
    /// Indicator types by indicator topic.
    /// </summary>
    public static readonly Dictionary<IndicatorTopics, IndicatorType[]> IndicatorTypesByIndicatorTopic = new()
    {
        { IndicatorTopics.OccupiedAreaPercent, [IndicatorType.OccupiedAreaPercent] },
        { IndicatorTopics.DetectionOccupancyProbability, [IndicatorType.DetectionProbability, IndicatorType.OccupancyProbability] },
        { IndicatorTopics.SpeciesDiversity, [IndicatorType.SpeciesRichness, IndicatorType.ShannonIndex, IndicatorType.SimpsonIndex] },
        { IndicatorTopics.RelativeUseByBiologicalGroup, [IndicatorType.RelativeUseIndex] },
        { IndicatorTopics.CentralRelationalIntensity, [IndicatorType.RelationalIntensity] },
        { IndicatorTopics.CollectiveActionParticipation, [IndicatorType.PersonCount] },
    };

    /// <summary>
    /// Indicators with species.
    /// </summary>
    public static readonly IndicatorTopics[] ObservationsWithSpecies =
    [
        IndicatorTopics.OccupiedAreaPercent,
        IndicatorTopics.DetectionOccupancyProbability,
        IndicatorTopics.RelativeUseByBiologicalGroup,
    ];

    /// <summary>
    /// Observations with confidence interval.
    /// </summary>
    public static readonly IndicatorTopics[] ObservationsWithConfidenceInterval =
    [
        IndicatorTopics.DetectionOccupancyProbability,
        IndicatorTopics.SpeciesDiversity,
    ];

    /// <summary>
    /// Observations with date range.
    /// </summary>
    public static readonly IndicatorTopics[] ObservationsWithDateRange =
    [
        IndicatorTopics.RelativeUseByBiologicalGroup,
    ];

    /// <summary>
    /// Observations with predefined categories.
    /// </summary>
    public static readonly IndicatorTopics[] ObservationsWithPredefinedCategories =
    [
        IndicatorTopics.SpeciesDiversity,
        IndicatorTopics.CentralRelationalIntensity,
        IndicatorTopics.CollectiveActionParticipation,
    ];

    /// <summary>
    /// Observations with integer values.
    /// </summary>
    public static readonly IndicatorTopics[] ObservationsWithIntegerValues =
    [
        IndicatorTopics.RelativeUseByBiologicalGroup,
        IndicatorTopics.CollectiveActionParticipation,
    ];
}
