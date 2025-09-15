namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Interfaces;

using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Config.General;

public interface IReportConfig<T>
{
    void Configure(ReportMapBuilder<T> builder);
}
