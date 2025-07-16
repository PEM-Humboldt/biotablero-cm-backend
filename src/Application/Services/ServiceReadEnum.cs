namespace IAVH.BioTablero.CM.Application.Services;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Helpers.General;

/// <summary>
/// Read enum service 
/// </summary>
public class ServiceReadEnum<TEnum> : IServiceReadEnum<TEnum>
    where TEnum : Enum
{
    /// <summary>
    /// Get all elements
    /// </summary>
    /// <returns>Process result</returns>
    public CustomWebResponse GetAll()
    {
        var enumData = Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(t => new EnumEntityDto<TEnum>(t));

        return new CustomWebResponse()
        {
            ResponseBody = enumData,
        };
    }
}