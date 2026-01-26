namespace IAVH.BioTablero.CM.Application.Domain;

using System.Collections.Generic;

/// <summary>
/// General OData response.
/// </summary>
/// <typeparam name="T">Class type.</typeparam>
public class ODataResponse<T>
    where T : class
{
    /// <summary>
    /// Total items.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Data list.
    /// </summary>
    public List<T> DataList { get; set; }
}
