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
    public const string Url = @"^[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)$";

    /// <summary>
    /// Regular expression for keyword list.
    /// <example>
    /// Valid keyword list:
    /// - Naturaleza,Colibrí
    /// - Champiñón,Guanábana,Ñame,Plátano
    /// - COP16,G20
    /// - Yuca
    /// </example>
    /// </summary>
    public const string Keywords = @"^([A-Za-z0-9\u00d1ÁÉÍÓÚ\u00f1áéíóú]+(,){1}){0,3}([A-Za-z0-9\u00d1ÁÉÍÓÚ\u00f1áéíóú]+){1}$";

    /// <summary>
    /// Regular expression for YouTube video Url.
    /// <example>
    /// Valid video urls:
    /// - https://www.youtube.com/watch?v=I2Rz6cHdoHY
    /// - https://www.youtube.com/watch?v=B8lUy-dJ0zY
    /// </example>
    /// </summary>
    public const string YouTubeVideoUrl = @"^(https:\/\/www\.youtube\.com\/watch\?v=([a-z]|[A-Z]|[0-9]|-|_){8,11})(&.*)*$";
}
