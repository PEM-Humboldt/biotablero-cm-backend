namespace IAVH.BioTablero.CM.Application.Domain;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// General OData response.
/// </summary>
/// <typeparam name="T">Class type.</typeparam>
[method: SetsRequiredMembers]
public class ODataResponse<T>()
    where T : class
{
    /// <summary>
    /// Total items.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Data list.
    /// </summary>
    public required List<T> DataList { get; set; } = [];
}
