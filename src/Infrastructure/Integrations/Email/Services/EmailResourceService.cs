namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Email.Services;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Email;
using IAVH.BioTablero.CM.Application.Interfaces.Services.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Models.Email;

/// <summary>
/// Email service for Resource entities.
/// </summary>
public class EmailResourceService : IEmailResourceService
{
    private readonly IWebViewTools webViewTools;
    private readonly IEmailService emailService;
    private readonly IIamService iamService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="webViewTools">Web View Tools.</param>
    /// <param name="emailService">Email service.</param>
    /// <param name="iamService">IAM service.</param>
    public EmailResourceService(
        IWebViewTools webViewTools,
        IEmailService emailService,
        IIamService iamService)
    {
        this.webViewTools = webViewTools;
        this.emailService = emailService;
        this.iamService = iamService;
    }

    /// <inheritdoc/>
    public async Task<bool> SendNotificationUpdateResource(Resource resource, string userName, string[] initiativeUsers, CancellationToken ct = default)
    {
        var emailData = new UpdateResourceEmailData
        {
            ResourceName = resource.Name,
            EditorUserName = userName,
        };

        var externalUsersData = await iamService.GetUsersDataAsync(initiativeUsers, ct);

        var receivers = initiativeUsers
            .Select(e => new CustomEmailAddress(e))
            .ToArray();

        var htmlBody = await webViewTools.RenderViewToStringAsync("UpdateResource", emailData);

        var response = await emailService.SendEmailAsync(emailData.Subject, receivers, null, htmlBody, ct);

        return !string.IsNullOrEmpty(response);
    }
}
