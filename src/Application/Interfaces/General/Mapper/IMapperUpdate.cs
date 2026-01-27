namespace IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Mapper Update interface.
/// </summary>
/// <typeparam name="TE">Entity type.</typeparam>
/// <typeparam name="TDto">DTO class type.</typeparam>
public interface IMapperUpdate<TE, TDto>
    where TE : class, IAggregateRoot
    where TDto : class, IDto
{
    /// <summary>
    /// Update entity data.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <param name="dto">DTO data.</param>
    void Update(TE entity, TDto dto);
}
