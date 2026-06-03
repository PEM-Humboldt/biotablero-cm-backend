namespace IAVH.BioTablero.CM.Application.Mappings.Users;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Users;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

/// <summary>
/// User mappings.
/// </summary>
public class ExternalUserBaseMappings : MapperRead<ExternalUser, ExternalUserBaseDto>
{
    /// <inheritdoc/>
    public override ExternalUserBaseDto Map(ExternalUser entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Username = entity.Username,
            FullName = entity.FullName,
            Email = entity.Email,
            Picture = entity.Picture,
        };
    }
}
