namespace IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Mapper Create interface.
/// </summary>
/// <typeparam name="TE">Entity type.</typeparam>
/// <typeparam name="TDto">DTO class type.</typeparam>
public interface IMapperCreate<TE, TDto>
    where TE : class, IAggregateRoot
    where TDto : class, IDto
{
    /// <summary>
    /// Map from DTO to entity. Useful for creating data.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
    TE Map(TDto dto);
}
