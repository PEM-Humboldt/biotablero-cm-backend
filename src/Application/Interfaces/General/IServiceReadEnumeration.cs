namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System;

using IAVH.BioTablero.CM.Core.Helpers.General;

/// <summary>
/// Read enum service
/// </summary>
/// <typeparam name="TEnum">Enum type</typeparam>
public interface IServiceReadEnumeration<TEnum>
    where TEnum : Enum
{
    /// <summary>
    /// Get all elements
    /// </summary>
    /// <returns>Process result</returns>
    CustomWebResponse GetAll();
}
