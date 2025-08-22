namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Email;

using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

/// <summary>
/// Email service interface.
/// </summary>
public class EmailService : IEmailService
{
    private readonly string smtpHost;
    private readonly int smtpPort;
    private readonly string smtpFrom;
    private readonly string smtpUser;
    private readonly string smtpPass;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <exception cref="InvalidCastException">SMTP Port cast exception.</exception>
    public EmailService()
    {
        smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST");
        smtpFrom = Environment.GetEnvironmentVariable("SMTP_FROM");
        smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
        smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS");

        var smtpStr = Environment.GetEnvironmentVariable("SMTP_PORT");
        if (int.TryParse(smtpStr, out smtpPort))
        {
            throw new InvalidCastException($"Invalid SMTP port value: '{smtpStr}'");
        }
    }

    /// <summary>
    /// Send email.
    /// </summary>
    /// <param name="subject">Email subject.</param>
    /// <param name="receivers">Email receivers list.</param>
    /// <param name="body">Email body.</param>
    /// <param name="isHtml">Email body HTML format flag.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task SendEmail(string subject, MailAddress[] receivers, string body, bool isHtml = true, CancellationToken ct = default)
    {
        using var client = new SmtpClient(smtpHost, smtpPort);
        client.Credentials = new NetworkCredential(smtpUser, smtpPass);
        client.EnableSsl = true;

        using var mail = new MailMessage
        {
            From = new MailAddress(smtpFrom),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml,
        };

        foreach (var receiver in receivers)
        {
            mail.To.Add(receiver);
        }

        await client.SendMailAsync(mail, ct);
    }
}
