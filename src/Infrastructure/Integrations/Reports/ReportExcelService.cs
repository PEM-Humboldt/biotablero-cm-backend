namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Reports;

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

using ClosedXML.Excel;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Config.General;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Interfaces;

/// <summary>
/// Logs report service interface (Excel).
/// </summary>
/// <typeparam name="TDto">DTO class type.</typeparam>
/// <param name="serviceProvider">Service provider.</param>
public class ReportExcelService<TDto>(IServiceProvider serviceProvider) : IReportService<TDto>
    where TDto : class, IDto
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    /// <summary>
    /// Generate report.
    /// </summary>
    /// <param name="dataList">DTO object list.</param>
    /// <param name="sheetName">Sheet name.</param>
    /// <returns>Report data.</returns>
    public byte[] GenerateReport(TDto[] dataList, string sheetName = "report")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        // Get config mappings
        var map = serviceProvider.GetService(typeof(IReportConfig<TDto>)) as IReportConfig<TDto>;
        var builder = new ReportMapBuilder<TDto>();
        map?.Configure(builder);
        var mappings = builder.GetMappings();

        var properties = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var orderedProps = properties
            .OrderBy(p => mappings.ContainsKey(p.Name) ? mappings[p.Name].Index : int.MaxValue)
            .ToList();

        // Add headers
        int col = 1;
        foreach (var prop in orderedProps)
        {
            if (mappings.ContainsKey(prop.Name) && mappings[prop.Name].Visible)
            {
                var header = mappings.ContainsKey(prop.Name)
                    ? mappings[prop.Name].Header
                    : prop.Name;

                worksheet.Cell(1, col).Value = header;
                col++;
            }
        }

        // Insert data
        int row = 2;
        foreach (var item in dataList)
        {
            col = 1;
            foreach (var prop in orderedProps)
            {
                if (mappings.ContainsKey(prop.Name) && mappings[prop.Name].Visible)
                {
                    var value = prop.GetValue(item);

                    if (value == null)
                    {
                        worksheet.Cell(row, col).Value = string.Empty;
                    }
                    else if (value is DateTime dt)
                    {
                        worksheet.Cell(row, col).Value = dt;
                    }
                    else if (value is bool b)
                    {
                        worksheet.Cell(row, col).Value = b;
                    }
                    else if (value is int or long or double or decimal or float)
                    {
                        worksheet.Cell(row, col).Value = Convert.ToDouble(value, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        worksheet.Cell(row, col).Value = value.ToString();
                    }

                    col++;
                }
            }

            row++;
        }

        // Create table
        var lastRow = Math.Max(1, 1 + dataList.Length);
        var tableRange = worksheet.Range(1, 1, lastRow, mappings.Count);
        tableRange.CreateTable();

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
