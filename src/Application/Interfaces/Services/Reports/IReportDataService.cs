namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Reports;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Reports;

/// <summary>
/// Report Data service interface.
/// </summary>
public interface IReportDataService : IRead<ReportData, int>, IAdd<ReportDataDto>
{
}
