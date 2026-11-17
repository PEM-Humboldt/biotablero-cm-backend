namespace IAVH.BioTablero.CM.Application.Services.General;

using System;
using System.Collections.Generic;
using System.Linq;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Read enum service.
/// </summary>
/// <typeparam name="TEnum">Enum type.</typeparam>
public class ServiceReadEnumeration<TEnum> : IReadEnumeration<TEnum>
    where TEnum : Enum
{
    /// <inheritdoc/>
    public IEnumerable<EnumEntityDto<TEnum>> GetEnumerable() => Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(t => new EnumEntityDto<TEnum>(t));

    /// <inheritdoc/>
    public CustomWebResponse GetAll() => new()
    {
        ResponseBody = GetEnumerable(),
    };
}
