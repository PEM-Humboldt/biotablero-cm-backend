namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System;


using IAVH.BioTablero.CM.Core.Helpers.General;

/// <summary>
/// Read enum service interface 
/// </summary>
public interface IServiceReadEnum<TEnum>
    where TEnum : Enum
{
    /// <summary>
    /// Get all elements
    /// </summary>
    /// <returns>Process result</returns>
    public CustomWebResponse GetAll();
}
