namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Email;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Domain.Models.Email;

using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Email service.
/// </summary>
public class EmailService : IEmailService
{
    private readonly ILogger logger;
    private readonly string smtpHost;
    private readonly int smtpPort;
    private readonly string smtpFrom;
    private readonly string smtpFromName;
    private readonly string smtpUser;
    private readonly string smtpPass;
    private readonly bool smtpEnableSsl;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">System logger.</param>
    /// <exception cref="InvalidCastException">SMTP Port cast exception.</exception>
    public EmailService(ILogger logger)
    {
        this.logger = logger;
        smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST");
        smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
        smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS");
        smtpFrom = Environment.GetEnvironmentVariable("SMTP_FROM");
        smtpFromName = Environment.GetEnvironmentVariable("SMTP_FROM_NAME");

        var smtpStr = Environment.GetEnvironmentVariable("SMTP_PORT");
        if (!int.TryParse(smtpStr, out smtpPort))
        {
            throw new InvalidCastException($"Invalid SMTP port value: '{smtpStr}'");
        }

        var smtpEnableSslStr = Environment.GetEnvironmentVariable("SMTP_ENABLED_SSL");
        if (!bool.TryParse(smtpEnableSslStr, out smtpEnableSsl))
        {
            smtpEnableSsl = true;
        }
    }

    /// <summary>
    /// Send email.
    /// </summary>
    /// <param name="subject">Email subject.</param>
    /// <param name="receivers">Email receivers list.</param>
    /// <param name="hiddenReceivers">Email hidden receivers list.</param>
    /// <param name="body">Email body.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<string> SendEmailAsync(string subject, CustomEmailAddress[] receivers, CustomEmailAddress[] hiddenReceivers, string body, CancellationToken ct = default)
    {
        var options = SecureSocketOptions.StartTls;

        if (!smtpEnableSsl)
        {
            options = SecureSocketOptions.None;
        }

        using var client = new SmtpClient();

        await client.ConnectAsync(smtpHost, smtpPort, options, ct);

        if (!string.IsNullOrEmpty(smtpUser))
        {
            await client.AuthenticateAsync(smtpUser, smtpPass, ct);
        }

        var builder = new BodyBuilder
        {
            HtmlBody = body,
        };

        using var mail = new MimeMessage()
        {
            Subject = subject,
            Body = builder.ToMessageBody(),
        };
        mail.From.Add(new MailboxAddress(smtpFromName, smtpFrom));
        mail.Cc.AddRange(receivers.Select(r => new MailboxAddress(r.Name, r.Email)));

        if (hiddenReceivers?.Length > 0)
        {
            mail.Bcc.AddRange(hiddenReceivers.Select(r => new MailboxAddress(r.Name, r.Email)));
        }

        var serverResponse = await client.SendAsync(mail, ct);

        var emailData = new
        {
            Subject = subject,
            From = smtpFrom,
            Body = body,
            Cc = receivers,
            Bcc = hiddenReceivers,
            ServerResponse = serverResponse,
        };

        logger
            .ForContext("CustomRecord", true)
            .ForContext("Type", (int)LogType.System)
            .Information("Sended email: {@EmailData}", emailData);

        return serverResponse;
    }
}
