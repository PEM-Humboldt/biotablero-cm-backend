namespace IAVH.BioTablero.CM.Application.Mappings.Reports;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Reports;

/// <summary>
/// Report Data mappings.
/// </summary>
public class ReportDataMappings : MapperRead<ReportData, ReportDataDto>, IMapperCreateAndRead<ReportData, ReportDataDto>
{
    /// <inheritdoc/>
    public override ReportDataDto Map(ReportData? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            CreationDate = entity.CreationDate.ToUniversalTime(),
            UserName = entity.UserName,
            Description = entity.Description,
            Data = entity.Data,
        };
    }

    /// <inheritdoc/>
    public ReportData Map(ReportDataDto? dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            CreationDate = dto.CreationDate ?? DateTimeOffset.UtcNow,
            UserName = dto.UserName ?? string.Empty,
            Description = dto.Description,
            Data = dto.Data,
        };
    }
}
