namespace IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Report service interface.
/// </summary>
/// <typeparam name="TDto">DTO class type.</typeparam>
public interface IReportService<TDto>
    where TDto : class, IDto
{
    /// <summary>
    /// Generate report.
    /// </summary>
    /// <param name="dataList">DTO object list.</param>
    /// <param name="sheetName">Sheet name.</param>
    /// <returns>Report data.</returns>
    byte[] GenerateReport(IEnumerable<TDto> dataList, string sheetName = "report");
}
