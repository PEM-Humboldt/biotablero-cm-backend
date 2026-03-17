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
    private readonly SmtpConfigData smtpData;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">System logger.</param>
    /// <exception cref="InvalidCastException">SMTP Port cast exception.</exception>
    public EmailService(ILogger logger)
    {
        this.logger = logger;
        smtpData = InitSmtpData();
    }

    /// <inheritdoc/>
    public async Task<string> SendEmailAsync(string subject, CustomEmailAddress[] receivers, CustomEmailAddress[] hiddenReceivers, string body, CancellationToken ct = default)
    {
        var options = SecureSocketOptions.StartTls;

        if (!smtpData.EnableSsl)
        {
            options = SecureSocketOptions.None;
        }

        using var client = new SmtpClient();

        await client.ConnectAsync(smtpData.Host, smtpData.Port, options, ct);

        if (!string.IsNullOrEmpty(smtpData.User))
        {
            await client.AuthenticateAsync(smtpData.User, smtpData.Password, ct);
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
        mail.From.Add(new MailboxAddress(smtpData.FromName, smtpData.From));
        mail.Cc.AddRange(receivers.Select(r => new MailboxAddress(r.Name, r.Email)));

        if (hiddenReceivers?.Length > 0)
        {
            mail.Bcc.AddRange(hiddenReceivers.Select(r => new MailboxAddress(r.Name, r.Email)));
        }

        var serverResponse = await client.SendAsync(mail, ct);

        var emailData = new
        {
            Subject = subject,
            From = smtpData.From,
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

    /// <summary>
    /// Initialize SMTP data.
    /// </summary>
    /// <returns>SMTP data object.</returns>
    public static SmtpConfigData InitSmtpData()
    {
        var smtpData = new SmtpConfigData()
        {
            Host = Environment.GetEnvironmentVariable("SMTP_HOST"),
            User = Environment.GetEnvironmentVariable("SMTP_USER"),
            Password = Environment.GetEnvironmentVariable("SMTP_PASS"),
            From = Environment.GetEnvironmentVariable("SMTP_FROM"),
            FromName = Environment.GetEnvironmentVariable("SMTP_FROM_NAME"),
        };

        var smtpStr = Environment.GetEnvironmentVariable("SMTP_PORT");
        if (!int.TryParse(smtpStr, out int smtpPort))
        {
            throw new InvalidCastException($"Invalid SMTP port value: '{smtpStr}'");
        }

        var smtpEnableSslStr = Environment.GetEnvironmentVariable("SMTP_ENABLED_SSL");
        if (!bool.TryParse(smtpEnableSslStr, out bool smtpEnableSsl))
        {
            smtpEnableSsl = true;
        }

        smtpData.Port = smtpPort;
        smtpData.EnableSsl = smtpEnableSsl;

        return smtpData;
    }
}
