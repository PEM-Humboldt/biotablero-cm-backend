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
    public const string Phone = @"^(([0-9]{7})|([36]{1}[0-9]{9}))$";

    /// <summary>
    /// Regular expression for urls.
    /// <example>
    /// Valid urls:
    /// - http://example.com/example
    /// - https://example.com/example
    /// </example>
    /// </summary>
    public const string Url = @"^(https?:\/\/)([\da-z\.-]+\.[a-z\.]{2,6}|[\d\.]+)([\/:?=&#]{1}[\da-z\.-]+)*[\/\?]?$";

    /// <summary>
    /// Regular expression for keyword list.
    /// <example>
    /// Valid keyword list:
    /// - Naturaleza,Colibrí
    /// - Champiñón,Guanábana,Ñame
    /// - COP16,G20
    /// </example>
    /// </summary>
    public const string Keywords = @"^([A-Z\u00d1ÁÉÍÓÚ]+[a-z0-9\u00f1áéíóú]+(,){1})*([A-Z\u00d1ÁÉÍÓÚ]+[a-z0-9\u00f1áéíóú]+)$";

    /// <summary>
    /// Regular expression for YouTube video Url.
    /// <example>
    /// Valid video urls:
    /// - https://www.youtube.com/watch?v=I2Rz6cHdoHY
    /// - https://www.youtube.com/watch?v=B8lUy-dJ0zY
    /// </example>
    /// </summary>
    public const string YouTubeVideoUrl = @"^(https:\/\/www\.youtube\.com\/watch\?v=([a-z]|[A-Z]|[0-9]|-){8,11})(&.*)*$";
}
