namespace IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Mapper Read and Update interface.
/// </summary>
/// <typeparam name="TE">Entity type.</typeparam>
/// <typeparam name="TDto">DTO class type.</typeparam>
public interface IMapperReadAndUpdate<TE, TDto> : IMapperRead<TE, TDto>, IMapperUpdate<TE, TDto>
    where TE : class, IAggregateRoot
    where TDto : class, IDto
{
}
