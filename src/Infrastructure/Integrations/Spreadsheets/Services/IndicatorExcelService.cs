#nullable enable
namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Spreadsheets.Services;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using ClosedXML.Excel;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Spreadsheets.Services;
using IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums;

/// <summary>
/// Indicator Excel service.
/// </summary>
/// <param name="logger">Logger.</param>
public class IndicatorExcelService(ILogger logger) : IIndicatorExcelService
{
    private readonly ILogger logger = logger;

    /// <inheritdoc/>
    public SpreadsheetReadResult<IndicatorsImportRow> GetFileData(IInputFile formFile)
    {
        var result = new SpreadsheetReadResult<IndicatorsImportRow>();

        if (formFile == null || formFile.Size == 0)
        {
            result.Errors.Add("Empty file");
            return result;
        }

        try
        {
            using var stream = formFile.OpenStream();
            using var workbook = new XLWorkbook(stream);

            var worksheet = workbook.Worksheet(1);

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                ValidateCellValue<int>(row, XlsxColumnIndex.IndicatorTypeId, result.Errors, out var indicatorTypeId);
                ValidateCellValue<int>(row, XlsxColumnIndex.MeasureUnitId, result.Errors, out var measureUnitId);
                ValidateCellValue<string>(row, XlsxColumnIndex.Department, result.Errors, out var departmentName);
                ValidateCellValue<string>(row, XlsxColumnIndex.Municipality, result.Errors, out var municipalityName);
                ValidateCellValue<string>(row, XlsxColumnIndex.Locality, result.Errors, out var localityName);
                ValidateCellValue<string>(row, XlsxColumnIndex.Year, result.Errors, out var year);
                ValidateCellValue<string>(row, XlsxColumnIndex.Month, result.Errors, out var month);
                ValidateCellValue<string>(row, XlsxColumnIndex.FinalYear, result.Errors, out var finalYear, true);
                ValidateCellValue<string>(row, XlsxColumnIndex.FinalMonth, result.Errors, out var finalMonth, true);
                ValidateCellValue<string>(row, XlsxColumnIndex.UpperGroupName, result.Errors, out var upperGroupName);
                ValidateCellValue<string>(row, XlsxColumnIndex.GroupName, result.Errors, out var groupName, true);
                ValidateCellValue<string>(row, XlsxColumnIndex.GroupDescription, result.Errors, out var groupDescription, true);
                ValidateCellValue<float>(row, XlsxColumnIndex.Value, result.Errors, out var value);
                ValidateCellValue<float?>(row, XlsxColumnIndex.UpperLimit, result.Errors, out var upperLimit);
                ValidateCellValue<float?>(row, XlsxColumnIndex.LowerLimit, result.Errors, out var lowerLimit);

                var validatedRow = new IndicatorsImportRow
                {
                    RowNumber = row.RowNumber(),
                    IndicatorTypeId = indicatorTypeId,
                    MeasureUnitId = measureUnitId,
                    DepartmentName = departmentName!,
                    MunicipalityName = municipalityName!,
                    LocalityName = localityName!,
                    Year = year,
                    Month = month,
                    FinalYear = finalYear,
                    FinalMonth = finalMonth,
                    UpperGroupName = upperGroupName!,
                    GroupName = groupName ?? string.Empty,
                    GroupDescription = groupDescription ?? string.Empty,
                    Value = value,
                    UpperLimit = upperLimit,
                    LowerLimit = lowerLimit,
                };

                result.Rows.Add(validatedRow);
            }
        }
        catch (InvalidOperationException ex)
        {
            logger.Error(ex, "Invalid .Xlsx file");
            result.Errors.Add("Invalid .Xlsx file. It may be corrupted.");
        }

        return result;
    }

    /// <summary>
    /// Validate cell value.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <param name="row">Row data.</param>
    /// <param name="columnIndex">Cell column index.</param>
    /// <param name="errors">Errors list.</param>
    /// <param name="value">Cell value.</param>
    /// <param name="isNullableString">Is nullable flag for string.</param>
    private void ValidateCellValue<TValue>(IXLRow row, XlsxColumnIndex columnIndex, List<string> errors, out TValue? value, bool isNullableString = false)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var isString = typeof(TValue) == typeof(string);
        var cellValueIsValid = ValidateCellValueData(row.Cell((int)columnIndex), out value);

        if (isString && value?.ToString() == string.Empty)
        {
            value = default;
        }

        var isNullable = !isString ? Nullable.GetUnderlyingType(typeof(TValue)) != null : isNullableString;

        if (!cellValueIsValid || (!isNullable && (value == null || (isString && string.IsNullOrWhiteSpace(value?.ToString())))))
        {
            errors.Add($"Row {row.RowNumber()}, Column '{columnIndex}': Invalid value.");
        }
    }

    /// <summary>
    /// Validate cell value data.
    /// </summary>
    /// <typeparam name="TCellValue">Cell value type.</typeparam>
    /// <param name="cell">Spreadsheet cell data.</param>
    /// <param name="value">Cell value output.</param>
    /// <returns>True if the cell value is valid for the specified type; otherwise, false.</returns>
    private bool ValidateCellValueData<TCellValue>(IXLCell cell, out TCellValue? value)
    {
        value = default;

        if (cell == null)
        {
            return false;
        }

        var targetType = typeof(TCellValue);
        var underlyingType = Nullable.GetUnderlyingType(targetType);

        if (underlyingType != null)
        {
            if (cell.IsEmpty())
            {
                value = default;
                return true;
            }

            targetType = underlyingType;
        }

        if (targetType == typeof(string))
        {
            if (cell.IsEmpty())
            {
                value = (TCellValue)(object)string.Empty;
                return true;
            }

            var text = cell.GetValue<string>()?.Trim();
            value = (TCellValue)(object)(text ?? string.Empty);
            return true;
        }

        if (cell.TryGetValue<TCellValue>(out var parsedValue))
        {
            value = parsedValue;
            return true;
        }

        if (!cell.IsEmpty())
        {
            try
            {
                var cellString = cell.GetValue<string>()?.Trim();
                if (!string.IsNullOrEmpty(cellString))
                {
                    var converted = Convert.ChangeType(cellString, targetType, CultureInfo.InvariantCulture);
                    value = (TCellValue)converted;
                    return true;
                }
            }
            catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
            {
                logger.Error(ex, "Validate Cell Value Data error {Cell}", cell);
            }
        }

        return false;
    }
}
