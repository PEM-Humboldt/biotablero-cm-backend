namespace IAVH.BioTablero.CM.Application.Utils;

using System.Globalization;

/// <summary>
/// Custom String utils.
/// </summary>
public static class StringUtils
{
    /// <summary>
    /// Capitalize string values.
    /// </summary>
    /// <param name="input">String input.</param>
    /// <returns>Capitalized string input.</returns>
    public static string Capitalize(this string input)
    {
        var currentCulture = CultureInfo.CurrentCulture;
        return currentCulture.TextInfo.ToTitleCase(input.ToLower(currentCulture));
    }
}
