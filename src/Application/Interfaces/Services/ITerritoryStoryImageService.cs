namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story Image service interface.
/// </summary>
public interface ITerritoryStoryImageService : IRead<TerritoryStoryImage, int>, IAdd<TerritoryStoryImageDto>, IUpdate<TerritoryStoryImageDto, int>, IDelete<int>
{
}
