namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Reports;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

using ClosedXML.Excel;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
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

        var orderedProperties = properties
            .OrderBy(p => GetPropertyIndex(p.Name, mappings))
            .ToList();

        // Add headers
        int col = 1;
        var propertyNames = orderedProperties
            .Select(prop => prop.Name);

        foreach (var propertyName in propertyNames)
        {
            if (mappings.ContainsKey(propertyName) && mappings[propertyName].Visible)
            {
                var header = GetPropertyHeader(propertyName, mappings);
                worksheet.Cell(1, col).Value = header;
                col++;
            }
        }

        // Insert data
        int row = 2;
        foreach (var item in dataList)
        {
            col = 1;
            foreach (var prop in orderedProperties)
            {
                if (mappings.ContainsKey(prop.Name) && mappings[prop.Name].Visible)
                {
                    var value = prop.GetValue(item);
                    AddCellData(worksheet.Cell(row, col), value);
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

    /// <summary>
    /// Get index from type property name.
    /// </summary>
    /// <param name="propertyName">Type property name.</param>
    /// <param name="mappings">Report mappings.</param>
    /// <returns>Property index.</returns>
    private static int GetPropertyIndex(string propertyName, IReadOnlyDictionary<string, ReportColumnConfig> mappings) =>
        mappings.ContainsKey(propertyName) ? mappings[propertyName].Index : int.MaxValue;

    /// <summary>
    /// Get header from type property name.
    /// </summary>
    /// <param name="propertyName">Type property name.</param>
    /// <param name="mappings">Report mappings.</param>
    /// <returns>Property header.</returns>
    private static string GetPropertyHeader(string propertyName, IReadOnlyDictionary<string, ReportColumnConfig> mappings) =>
        mappings.ContainsKey(propertyName) ? mappings[propertyName].Header : propertyName;

    /// <summary>
    /// Add data in spreadsheet cell.
    /// </summary>
    /// <param name="cell">Spreadsheet cell.</param>
    /// <param name="value">Spreadsheet cell value.</param>
    private static void AddCellData(IXLCell cell, object value)
    {
        if (value == null)
        {
            cell.Value = string.Empty;
        }
        else if (value is DateTime dt)
        {
            cell.Value = dt;
        }
        else if (value is bool b)
        {
            cell.Value = b;
        }
        else if (value is int or long or double or decimal or float)
        {
            cell.Value = Convert.ToDouble(value, CultureInfo.CurrentCulture);
        }
        else
        {
            cell.Value = value.ToString();
        }
    }
}
