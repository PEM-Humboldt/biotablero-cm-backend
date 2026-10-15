namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Central Actor mappings.
/// </summary>
public class CentralActorMappings() : MapperRead<CentralActor, CentralActorDto>
{
    /// <inheritdoc/>
    public override CentralActorDto Map(CentralActor? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Name = entity.Name,
        };
    }
}
