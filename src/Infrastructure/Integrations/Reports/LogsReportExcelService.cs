namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Reports;

using System.Collections.Generic;
using System.IO;

using ClosedXML.Excel;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;

/// <summary>
/// Logs report service interface (Excel).
/// </summary>
public class LogsReportExcelService : IReportService<LogDto>
{
    /// <summary>
    /// Generate report.
    /// </summary>
    /// <param name="dataList">DTO object list.</param>
    /// <returns>Report data.</returns>
    public byte[] GenerateReport(IEnumerable<LogDto> dataList)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Reporte");

        // Encabezados
        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "Fecha de creación";
        worksheet.Cell(1, 3).Value = "Usuario";
        worksheet.Cell(1, 4).Value = "IP origen";
        worksheet.Cell(1, 5).Value = "Navegador";
        worksheet.Cell(1, 6).Value = "Tipo";
        worksheet.Cell(1, 7).Value = "Mensaje";

        int row = 2;
        foreach (var item in dataList)
        {
            worksheet.Cell(row, 1).Value = item.Id.ToString();
            worksheet.Cell(row, 2).Value = item.TimeStamp;
            worksheet.Cell(row, 3).Value = item.UserName;
            worksheet.Cell(row, 4).Value = item.ClientIp;
            worksheet.Cell(row, 5).Value = item.ClientAgent;
            worksheet.Cell(row, 6).Value = item.Type.ToString();
            worksheet.Cell(row, 7).Value = item.Message;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
