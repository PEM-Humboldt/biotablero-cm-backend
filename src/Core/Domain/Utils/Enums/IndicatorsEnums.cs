namespace IAVH.BioTablero.CM.Core.Domain.Utils.Enums;

/// <summary>
/// Initiatives enumerations.
/// </summary>
public static class IndicatorsEnums
{
    #region Indicators

    /// <summary>
    /// Indicator topics.
    /// </summary>
    public enum IndicatorTopic
    {
        /// <summary>
        /// Percentage of the study area occupied by the species.
        /// </summary>
        OccupiedAreaPercent = 1,

        /// <summary>
        /// Probability of detection and occupancy of the species in the study area (without covariates).
        /// </summary>
        DetectionOccupancyProbability = 2,

        /// <summary>
        /// Species diversity in the study area.
        /// </summary>
        SpeciesDiversity = 3,

        /// <summary>
        /// Relative use of species by biological group.
        /// </summary>
        RelativeUseByBiologicalGroup = 4,

        /// <summary>
        /// Relational intensity index of the central actor.
        /// </summary>
        CentralRelationalIntensity = 5,

        /// <summary>
        /// Composition of participation in active collective action processes.
        /// </summary>
        CollectiveActionParticipation = 6,
    }

    /// <summary>
    /// Indicator types.
    /// </summary>
    public enum IndicatorType
    {
        /// <summary>
        /// Percentage of area occupied.
        /// </summary>
        OccupiedAreaPercent = 1,

        /// <summary>
        ///  Probability of detection.
        /// </summary>
        DetectionProbability = 2,

        /// <summary>
        /// Probability of occupancy.
        /// </summary>
        OccupancyProbability = 3,

        /// <summary>
        /// Species richness.
        /// </summary>
        SpeciesRichness = 4,

        /// <summary>
        /// Shannon index.
        /// </summary>
        ShannonIndex = 5,

        /// <summary>
        /// Simpson index.
        /// </summary>
        SimpsonIndex = 6,

        /// <summary>
        /// Relative use index.
        /// </summary>
        RelativeUseIndex = 7,

        /// <summary>
        /// Relational intensity.
        /// </summary>
        RelationalIntensity = 8,

        /// <summary>
        /// Number of people.
        /// </summary>
        PersonCount = 9,
    }

    /// <summary>
    /// Observation base categories.
    /// </summary>
    public enum ObservationBaseCategory
    {
        /// <summary>
        /// Species category type.
        /// </summary>
        Species = 1,

        /// <summary>
        /// Actor category type.
        /// </summary>
        Actor = 2,

        /// <summary>
        /// Gender category type.
        /// </summary>
        Gender = 3,

        /// <summary>
        /// Agre group category type.
        /// </summary>
        AgeGroup = 4,
    }

    #endregion

    #region Spreadsheets

    /// <summary>
    /// Spreadsheet column index.
    /// </summary>
    public enum XlsxColumnIndex
    {
        /// <summary>
        /// Observation name.
        /// </summary>
        ObservationName = 1,

        /// <summary>
        /// Indicator topic identifier.
        /// </summary>
        IndicatorTopicId = 2,

        /// <summary>
        /// Indicator Type identifier.
        /// </summary>
        IndicatorTypeId = 4,

        /// <summary>
        /// Department name.
        /// </summary>
        Department = 6,

        /// <summary>
        /// Municipality name.
        /// </summary>
        Municipality = 7,

        /// <summary>
        /// Locality name.
        /// </summary>
        Locality = 8,

        /// <summary>
        /// Year.
        /// </summary>
        Year = 9,

        /// <summary>
        /// Month.
        /// </summary>
        Month = 10,

        /// <summary>
        /// Final year.
        /// </summary>
        FinalYear = 11,

        /// <summary>
        /// Final month.
        /// </summary>
        FinalMonth = 12,

        /// <summary>
        /// Upper group name.
        /// </summary>
        UpperGroupName = 13,

        /// <summary>
        /// Group name.
        /// </summary>
        GroupName = 14,

        /// <summary>
        /// Group description.
        /// </summary>
        GroupDescription = 15,

        /// <summary>
        /// Indicator value.
        /// </summary>
        Value = 16,

        /// <summary>
        /// Value upper limit.
        /// </summary>
        LowerLimit = 17,

        /// <summary>
        /// Value lower limit.
        /// </summary>
        UpperLimit = 18,
    }

    #endregion
}
