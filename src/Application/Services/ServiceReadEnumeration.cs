namespace IAVH.BioTablero.CM.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Utils;

/// <summary>
/// Read enum service
/// </summary>
/// <typeparam name="TEnum">Enum type</typeparam>
public class ServiceReadEnumeration<TEnum> : IServiceReadEnumeration<TEnum>
    where TEnum : Enum
{
    /// <summary>
    /// Get all elements as IEnumerable
    /// </summary>
    /// <returns>IEnumerable list</returns>
    public IEnumerable<EnumEntityDto<TEnum>> GetEnumerable() => Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(t => new EnumEntityDto<TEnum>(t));

    /// <summary>
    /// Get all elements
    /// </summary>
    /// <returns>Process result</returns>
    public CustomWebResponse GetAll() => new()
    {
        ResponseBody = GetEnumerable(),
    };
}
