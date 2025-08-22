namespace IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Email service interface.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send email.
    /// </summary>
    /// <param name="subject">Email subject.</param>
    /// <param name="receivers">Email receivers list.</param>
    /// <param name="body">Email body.</param>
    /// <param name="isHtml">Email body HTML format flag.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task SendEmail(string subject, MailAddress[] receivers, string body, bool isHtml = true, CancellationToken ct = default);
}
