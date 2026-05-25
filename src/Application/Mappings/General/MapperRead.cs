namespace IAVH.BioTablero.CM.Application.Mappings.General;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Mapper Read base.
/// </summary>
/// <typeparam name="TE">Entity type.</typeparam>
/// <typeparam name="TDto">DTO class type.</typeparam>
public abstract class MapperRead<TE, TDto> : IMapperRead<TE, TDto>
    where TE : class, IAggregateRoot
    where TDto : class, IDto, new()
{
    /// <inheritdoc/>
    public virtual TDto Map(TE entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new();
    }

    /// <inheritdoc/>
    public virtual TDto MapOdata(TE entity) => Map(entity);
}
