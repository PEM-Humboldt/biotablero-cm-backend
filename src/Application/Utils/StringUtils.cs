namespace IAVH.BioTablero.CM.Application.Utils;

using System.Globalization;

/// <summary>
/// Custom String utils.
/// </summary>
public static class StringUtils
{
    /// <summary>
    /// Convert first string char to lower case.
    /// </summary>
    /// <param name="input">String input.</param>
    /// <returns>String input with the first char in lower case.</returns>
    public static string LowerCaseFirstChar(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return char.ToLowerInvariant(input[0]) + input[1..];
    }

    /// <summary>
    /// Capitalize string values.
    /// </summary>
    /// <param name="input">String input.</param>
    /// <returns>Capitalized string input.</returns>
    public static string CapitalizeFirstOnly(this string input)
    {
        var currentCulture = CultureInfo.CurrentCulture;

        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return char.ToUpper(input[0], currentCulture) + input[1..].ToLower(currentCulture);
    }
}
