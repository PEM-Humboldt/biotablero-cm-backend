namespace IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

using System.Collections.Generic;
using System.Text;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums;

using TopicTypes = Enums.IndicatorsEnums.TopicType;

/// <summary>
/// Constants for indicators.
/// </summary>
public static class IndicatorConstants
{
    /// <summary>
    /// General constants.
    /// </summary>
    public static class General
    {
        /// <summary>
        /// Topic date format.
        /// </summary>
        public static readonly CompositeFormat TopicDateFormat = CompositeFormat.Parse("{0}-{1}-01");
    }

    /// <summary>
    /// Indicator categories constants.
    /// </summary>
    public static class Categories
    {
        /// <summary>
        /// "Species" category name.
        /// </summary>
        public const string SpeciesCategoryName = "Especie";

        /// <summary>
        /// "Other species" category name.
        /// </summary>
        public const string OtherSpeciesCategoryName = "Otras especies";

        /// <summary>
        /// "Aggregate" category name.
        /// </summary>
        public const string AggregateCategoryName = "Agregado";
    }

    /// <summary>
    /// Validations constants.
    /// </summary>
    public static class Validations
    {
        /// <summary>
        /// Indicator types by indicator topic.
        /// </summary>
        public static readonly Dictionary<TopicTypes, IndicatorType[]> IndicatorTypesByTopicType = new()
        {
            { TopicTypes.OccupiedAreaPercent, [IndicatorType.OccupiedAreaPercent] },
            { TopicTypes.DetectionOccupancyProbability, [IndicatorType.DetectionProbability, IndicatorType.OccupancyProbability] },
            { TopicTypes.SpeciesDiversity, [IndicatorType.SpeciesRichness, IndicatorType.ShannonIndex, IndicatorType.SimpsonIndex] },
            { TopicTypes.RelativeUseByBiologicalGroup, [IndicatorType.RelativeUseIndex] },
            { TopicTypes.CentralRelationalIntensity, [IndicatorType.RelationalIntensity] },
            { TopicTypes.CollectiveActionParticipation, [IndicatorType.PersonCount] },
        };

        /// <summary>
        /// Topics with species.
        /// </summary>
        public static readonly TopicTypes[] TopicsWithSpecies =
        [
            TopicTypes.OccupiedAreaPercent,
            TopicTypes.DetectionOccupancyProbability,
            TopicTypes.RelativeUseByBiologicalGroup,
        ];

        /// <summary>
        /// Topics with confidence interval.
        /// </summary>
        public static readonly TopicTypes[] TopicsWithConfidenceInterval =
        [
            TopicTypes.DetectionOccupancyProbability,
            TopicTypes.SpeciesDiversity,
        ];

        /// <summary>
        /// Topics with date range.
        /// </summary>
        public static readonly TopicTypes[] TopicsWithDateRange =
        [
            TopicTypes.OccupiedAreaPercent,
            TopicTypes.DetectionOccupancyProbability,
            TopicTypes.SpeciesDiversity,
            TopicTypes.RelativeUseByBiologicalGroup,
            TopicTypes.CentralRelationalIntensity,
            TopicTypes.CollectiveActionParticipation,
        ];

        /// <summary>
        /// Topics with predefined categories.
        /// </summary>
        public static readonly TopicTypes[] TopicsWithPredefinedCategories =
        [
            TopicTypes.SpeciesDiversity,
            TopicTypes.CentralRelationalIntensity,
            TopicTypes.CollectiveActionParticipation,
        ];

        /// <summary>
        /// Topics with integer values.
        /// </summary>
        public static readonly TopicTypes[] TopicsWithIntegerValues =
        [
            TopicTypes.CollectiveActionParticipation,
        ];

        /// <summary>
        /// Topics with central actors.
        /// </summary>
        public static readonly TopicTypes[] TopicsWithCentralActors =
        [
            TopicTypes.CentralRelationalIntensity,
            TopicTypes.CollectiveActionParticipation,
        ];
    }
}
