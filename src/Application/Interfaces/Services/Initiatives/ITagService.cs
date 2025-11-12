namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Tag service interface.
/// </summary>
public interface ITagService : IRead<Tag, int>, IAdd<TagDto>, IUpdate<TagDto, int>, IDelete<int>
{
}
