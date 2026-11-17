namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Tags;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

/// <summary>
/// Tag service interface.
/// </summary>
public interface ITagService : IRead<Tag, int>, IAdd<TagDto>, IUpdate<TagDto, int>, IDelete<int>
{
}
