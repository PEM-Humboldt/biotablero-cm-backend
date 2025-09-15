namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Reports;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using ClosedXML.Excel;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Logs report service interface (Excel).
/// </summary>
/// <typeparam name="TDto">DTO class type.</typeparam>
public class ReportExcelService<TDto> : IReportService<TDto>
    where TDto : class, IDto
{
    /// <summary>
    /// Generate report.
    /// </summary>
    /// <param name="dataList">DTO object list.</param>
    /// <param name="sheetName">Sheet name.</param>
    /// <returns>Report data.</returns>
    public byte[] GenerateReport(IEnumerable<TDto> dataList, string sheetName = "report")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        var properties = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Add headers
        for (int col = 0; col < properties.Length; col++)
        {
            worksheet.Cell(1, col + 1).Value = properties[col].Name;
        }

        // Insert data
        worksheet.Cell(2, 1).InsertData(dataList);

        // Create table
        var lastRow = Math.Max(1, 1 + dataList.Count());
        var tableRange = worksheet.Range(1, 1, lastRow, properties.Length);
        tableRange.CreateTable();

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
