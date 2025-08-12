namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Utils;

/// <summary>
/// Read enum service
/// </summary>
/// <typeparam name="TEnum">Enum type</typeparam>
public interface IServiceReadEnumeration<TEnum>
    where TEnum : Enum
{
    /// <summary>
    /// Get all elements as IEnumerable
    /// </summary>
    /// <returns>IEnumerable list</returns>
    IEnumerable<EnumEntityDto<TEnum>> GetEnumerable();

    /// <summary>
    /// Get all elements
    /// </summary>
    /// <returns>Process result</returns>
    CustomWebResponse GetAll();
}
