namespace IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

using System.Collections.Generic;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums;

/// <summary>
/// Constants for indicators.
/// </summary>
public static class IndicatorConstants
{
    /// <summary>
    /// Unit measures by indicator type.
    /// </summary>
    public static readonly Dictionary<IndicatorType, IndicatorMeasureUnit[]> UnitMeasuresByIndicatorType = new()
    {
        { IndicatorType.OccupiedAreaPercent, [IndicatorMeasureUnit.OccupiedAreaPercent] },
        { IndicatorType.DetectionOccupancyProbability, [IndicatorMeasureUnit.DetectionProbability, IndicatorMeasureUnit.OccupancyProbability] },
        { IndicatorType.SpeciesDiversity, [IndicatorMeasureUnit.SpeciesRichness, IndicatorMeasureUnit.ShannonIndex, IndicatorMeasureUnit.SimpsonIndex] },
        { IndicatorType.RelativeUseByBiologicalGroup, [IndicatorMeasureUnit.RelativeUseIndex] },
        { IndicatorType.CentralRelationalIntensity, [IndicatorMeasureUnit.RelationalIntensity] },
        { IndicatorType.CollectiveActionParticipation, [IndicatorMeasureUnit.PersonCount] },
    };
}
