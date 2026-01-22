namespace IAVH.BioTablero.CM.Application.Utils;

using System.Linq;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Custom OData utils.
/// </summary>
public static class ODataUtils
{
    private static readonly ODataQuerySettings DefaultOdataQuerySettings = new()
    {
        HandleNullPropagation = HandleNullPropagationOption.True,
    };

    /// <summary>
    /// Add filter and order settings to OData query.
    /// </summary>
    /// <typeparam name="T">Class type.</typeparam>
    /// <param name="query">Linq Query.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="settings">OData query settings.</param>
    /// <returns>Modified Linq query.</returns>
    public static IQueryable<T> AddOdataQueryFilterAndOrder<T>(IQueryable<T> query, ODataQueryOptions<T> queryOptions, ODataQuerySettings settings = null)
        where T : class
    {
        settings ??= DefaultOdataQuerySettings;

        // Apply order and filter settings
        if (queryOptions?.Filter != null)
        {
            query = (IQueryable<T>)queryOptions.Filter.ApplyTo(query, settings);
        }

        if (queryOptions.OrderBy != null)
        {
            query = queryOptions.OrderBy.ApplyTo(query, settings);
        }

        return query;
    }

    /// <summary>
    /// Add pagination settings to OData query.
    /// </summary>
    /// <typeparam name="T">Class type.</typeparam>
    /// <param name="query">Linq Query.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="settings">OData query settings.</param>
    /// <returns>Modified Linq query.</returns>
    public static IQueryable<T> AddOdataQueryPagination<T>(IQueryable<T> query, ODataQueryOptions<T> queryOptions, ODataQuerySettings settings = null)
        where T : class
    {
        const int maxPageSize = 20;
        settings ??= DefaultOdataQuerySettings;

        if (queryOptions.Skip != null)
        {
            query = queryOptions.Skip.ApplyTo(query, settings);
        }

        var pageSize = queryOptions.Top?.Value ?? maxPageSize;
        if (pageSize > maxPageSize)
        {
            pageSize = maxPageSize;
        }

        query = query.Take(pageSize);

        return query;
    }
}
