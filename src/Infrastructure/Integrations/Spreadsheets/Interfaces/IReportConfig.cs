namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Spreadsheets.Interfaces;

using IAVH.BioTablero.CM.Infrastructure.Integrations.Spreadsheets.Config.General;

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
