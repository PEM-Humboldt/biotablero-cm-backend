namespace IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

using System;
using System.Globalization;

/// <summary>
/// General purpose constants.
/// </summary>
public static class GeneralConstants
{
    /// <summary>
    /// Date format.
    /// </summary>
    public const string DateFormat = "yyyy-MM-dd";

    /// <summary>
    /// Datetime format.
    /// </summary>
    public const string DatetimeFormat = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// Default format provider.
    /// </summary>
    public static readonly IFormatProvider DefaultFormatProvider = CultureInfo.InvariantCulture;
}
