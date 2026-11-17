namespace IAVH.BioTablero.CM.Core.Domain.Models.Email;

/// <summary>
/// SMTP configuration data.
/// </summary>
public class SmtpConfigData
{
    /// <summary>
    /// SMTP Host.
    /// </summary>
    public required string Host { get; set; }

    /// <summary>
    /// SMTP Port.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Enable SSL flag.
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// SMTP User.
    /// </summary>
    public required string User { get; set; }

    /// <summary>
    /// /SMTP Password.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Sender's email.
    /// </summary>
    public required string From { get; set; }

    /// <summary>
    /// Sender's name.
    /// </summary>
    public string? FromName { get; set; }
}
