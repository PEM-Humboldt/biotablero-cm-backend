namespace IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Mapper Create and Read interface.
/// </summary>
/// <typeparam name="TE">Entity type.</typeparam>
/// <typeparam name="TDto">DTO class type.</typeparam>
public interface IMapperCreateAndRead<TE, TDto> : IMapperRead<TE, TDto>, IMapperCreate<TE, TDto>
    where TE : class, IAggregateRoot
    where TDto : class, IDto
{
}
