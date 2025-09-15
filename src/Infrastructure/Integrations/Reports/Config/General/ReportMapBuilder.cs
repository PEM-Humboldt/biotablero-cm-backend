namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Config.General;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

public class ReportMapBuilder<T>
{
    private readonly Dictionary<string, ReportColumnConfig> mappings = [];

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

    internal IReadOnlyDictionary<string, ReportColumnConfig> GetMappings() => mappings;
}
