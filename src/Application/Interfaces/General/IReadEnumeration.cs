namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

/// <summary>
/// Read enum interface (for services).
/// </summary>
/// <typeparam name="TEnum">Enum type.</typeparam>
public interface IReadEnumeration<TEnum>
    where TEnum : Enum
{
    /// <summary>
    /// Get all elements as IEnumerable.
    /// </summary>
    /// <returns>IEnumerable list.</returns>
    IEnumerable<EnumEntityDto<TEnum>> GetEnumerable();

    /// <summary>
    /// Get all elements.
    /// </summary>
    /// <returns>Process result.</returns>
    CustomWebResponse GetAll();
}
