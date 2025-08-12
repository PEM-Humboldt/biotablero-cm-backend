namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// General mapper interface
/// </summary>
/// <typeparam name="TE">Entity type</typeparam>
/// <typeparam name="TDto">DTO class type</typeparam>
public interface IMapper<TE, TDto>
    where TE : class, IAggregateRoot
    where TDto : class, IDto
{
    /// <summary>
    /// Map from entity to DTO
    /// </summary>
    /// <param name="entity">Entity data</param>
    /// <returns>DTO data</returns>
    TDto Map(TE entity);

    /// <summary>
    /// Map from DTO to entity
    /// </summary>
    /// <param name="dto">DTO data</param>
    /// <returns>Entity data</returns>
    TE Map(TDto dto);
}
