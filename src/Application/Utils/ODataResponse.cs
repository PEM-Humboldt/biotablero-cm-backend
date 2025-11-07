namespace IAVH.BioTablero.CM.Application.Utils;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// General OData response.
/// </summary>
/// <typeparam name="TE">Entity type.</typeparam>
public class ODataResponse<TE>
    where TE : IAggregateRoot
{
    /// <summary>
    /// Total items.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Data list.
    /// </summary>
    public List<TE> DataList { get; set; }
}
