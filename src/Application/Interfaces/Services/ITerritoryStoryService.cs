namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story service interface.
/// </summary>
public interface ITerritoryStoryService : IRead<TerritoryStory, int>, IAdd<TerritoryStoryDto>, IUpdate<TerritoryStoryDto, int>, IDisable<int>
{
}
