namespace IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

/// <summary>
/// Constants for regular expressions.
/// </summary>
public static class RegExprConstants
{
    /// <summary>
    /// Regular expression for phone numbers.
    /// <example>
    /// Valid phone numbers:
    /// - 3055555555
    /// - 6015555555
    /// - 7785555
    /// </example>
    /// </summary>
    public const string Phone = "^(([0-9]{7})|([36]{1}[0-9]{9}))$";
}
