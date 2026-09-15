namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Observation Version service interface.
/// </summary>
public interface IObservationVersionService : IRead<ObservationVersion, int>, IUpdate<ObservationVersionDto, int>
{
}
