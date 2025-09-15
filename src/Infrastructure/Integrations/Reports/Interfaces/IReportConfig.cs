namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Interfaces;

using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Config.General;

/// <summary>
/// Custom report configuration.
/// </summary>
/// <typeparam name="T">Report entity type.</typeparam>
public interface IReportConfig<T>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    void Configure(ReportMapBuilder<T> builder);
}
