namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Tag service interface.
/// </summary>
public interface ITagService : IServiceRead<Tag, TagDto, int>, IServiceAdd<TagDto>, IServiceUpdate<TagDto, int>, IServiceDelete<int>
{
}
