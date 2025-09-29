namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Config.General;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

/// <summary>
/// Custom report map builder.
/// </summary>
/// <typeparam name="T">Class type.</typeparam>
public class ReportMapBuilder<T>
{
    private readonly Dictionary<string, ReportColumnConfig> mappings = [];

    /// <summary>
    /// Define class property.
    /// </summary>
    /// <typeparam name="TProp">Class property type.</typeparam>
    /// <param name="property">Class property.</param>
    /// <param name="header">Property header.</param>
    /// <param name="index">Property index.</param>
    /// <param name="visible">Property visible flag.</param>
    /// <returns>Report map builder.</returns>
    /// <exception cref="ArgumentException">Property argument exception.</exception>
    public ReportMapBuilder<T> Property<TProp>(
        Expression<Func<T, TProp>> property,
        string header = null,
        int? index = null,
        bool visible = false)
    {
        if (property.Body is not MemberExpression member)
        {
            throw new ArgumentException("The expression must be a property.");
        }

        var name = member.Member.Name;
        mappings[name] = new ReportColumnConfig
        {
            Header = header ?? name,
            Index = index ?? int.MaxValue,
            Visible = visible,
        };

        return this;
    }

    /// <summary>
    /// Get report map builder mappings.
    /// </summary>
    /// <returns>Report map builder mappings.</returns>
    internal IReadOnlyDictionary<string, ReportColumnConfig> GetMappings() => mappings;
}
