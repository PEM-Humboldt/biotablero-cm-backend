namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story Video service interface.
/// </summary>
public interface ITerritoryStoryVideoService : IRead<TerritoryStoryVideo, int>, IAdd<TerritoryStoryVideoDto>, IUpdate<TerritoryStoryVideoDto, int>, IDelete<int>
{
}
