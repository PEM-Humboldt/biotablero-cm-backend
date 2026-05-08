namespace IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Mapper read interface.
/// </summary>
/// <typeparam name="TE">Entity type.</typeparam>
/// <typeparam name="TDto">DTO class type.</typeparam>
public interface IMapperRead<TE, TDto>
    where TE : class, IAggregateRoot
    where TDto : class, IDto
{
    /// <summary>
    /// Map from entity to DTO. Useful for reading detailed data.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    TDto Map(TE entity);

    /// <summary>
    /// Map from entity to DTO for OData. Useful for reading simplified data.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    TDto MapOdata(TE entity);
}
