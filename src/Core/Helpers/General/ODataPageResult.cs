using System;
using System.Collections.Generic;

namespace IAVH.BioTablero.CM.Core.Helpers.General;

public class ODataPageResult<T>
    where T : notnull
{
    public ODataPageResult(IEnumerable<T> items, Uri nextPageLink, long count)
    {
        Value = items;
        NextPageLink = nextPageLink;
        Count = count;
    }

    public IEnumerable<T> Value { get; set; }
    public long Count { get; set; }
    public Uri NextPageLink { get; }
}