namespace IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Models.Email;

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
    /// <param name="hiddenReceivers">Email hidden receivers list.</param>
    /// <param name="body">Email body.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<string> SendEmailAsync(string subject, CustomEmailAddress[] receivers, CustomEmailAddress[] hiddenReceivers, string body, CancellationToken ct = default);
}
