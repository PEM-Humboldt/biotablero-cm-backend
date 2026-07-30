namespace IAVH.BioTablero.CM.Core.Domain.Utils.Enums;

/// <summary>
/// Initiatives enumerations.
/// </summary>
public static class IndicatorsEnums
{
    #region Indicators

    /// <summary>
    /// Indicator types.
    /// </summary>
    public enum IndicatorType
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
    /// Indicator Measure Units.
    /// </summary>
    public enum IndicatorMeasureUnit
    {
        /// <summary>
        /// Percentage of area occupied.
        /// </summary>
        OccupiedAreaPercent,

        /// <summary>
        ///  Probability of detection.
        /// </summary>
        DetectionProbability,

        /// <summary>
        /// Probability of occupancy.
        /// </summary>
        OccupancyProbability,

        /// <summary>
        /// Species richness.
        /// </summary>
        SpeciesRichness,

        /// <summary>
        /// Shannon index.
        /// </summary>
        ShannonIndex,

        /// <summary>
        /// Simpson index.
        /// </summary>
        SimpsonIndex,

        /// <summary>
        /// Relative use index.
        /// </summary>
        RelativeUseIndex,

        /// <summary>
        /// Relational intensity.
        /// </summary>
        RelationalIntensity,

        /// <summary>
        /// Number of people.
        /// </summary>
        PersonCount,
    }

    #endregion

    #region Spreadsheets

    /// <summary>
    /// Spreadsheet column index.
    /// </summary>
    public enum XlsxColumnIndex
    {
        /// <summary>
        /// Indicator type identifier.
        /// </summary>
        IndicatorTypeId = 1,

        /// <summary>
        /// Measure unit identifier.
        /// </summary>
        MeasureUnitId = 3,

        /// <summary>
        /// Department name.
        /// </summary>
        Department = 5,

        /// <summary>
        /// Municipality name.
        /// </summary>
        Municipality = 6,

        /// <summary>
        /// Locality name.
        /// </summary>
        Locality = 7,

        /// <summary>
        /// Year.
        /// </summary>
        Year = 8,

        /// <summary>
        /// Month.
        /// </summary>
        Month = 9,

        /// <summary>
        /// Upper group name.
        /// </summary>
        UpperGroupName = 10,

        /// <summary>
        /// Group name.
        /// </summary>
        GroupName = 11,

        /// <summary>
        /// Group description.
        /// </summary>
        GroupDescription = 12,

        /// <summary>
        /// Indicator value.
        /// </summary>
        Value = 13,

        /// <summary>
        /// Value upper limit.
        /// </summary>
        UpperLimit = 14,

        /// <summary>
        /// Value lower limit.
        /// </summary>
        LowerLimit = 15,
    }

    #endregion
}
