namespace IAVH.BioTablero.CM.WebApi.Controllers.Mvc;

using System.Linq;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Core.Domain.Utils.Email;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Email Templates controller.
/// </summary>
[Route("Email")]
[ApiExplorerSettings(IgnoreApi = true)]
public class EmailTemplatesController : Controller
{
    private readonly IWebViewTools webViewTools;
    private readonly IEmailService emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailTemplatesController"/> class.
    /// </summary>
    /// <param name="webViewTools">Web view tools service.</param>
    /// <param name="emailService">Email service.</param>
    public EmailTemplatesController(IWebViewTools webViewTools, IEmailService emailService)
    {
        this.webViewTools = webViewTools;
        this.emailService = emailService;
    }

    /// <summary>
    /// Get default view.
    /// </summary>
    /// <returns>Default view.</returns>
    [HttpGet]
    [Route("Default")]
    public IActionResult Default() => View();

    /// <summary>
    /// Get role assignment view.
    /// </summary>
    /// <param name="content">Optional content to preview.</param>
    /// <returns>Role assignment view.</returns>
    [HttpGet]
    [Route("RoleAssignment")]
    public IActionResult RoleAssignment(string content = null)
    {
        var model = !string.IsNullOrEmpty(content)
            ? new DefaultEmailData { Content = content }
            : null;

        return View(model);
    }

    /// <summary>
    /// Get user removal view.
    /// </summary>
    /// <param name="content">Optional content to preview.</param>
    /// <returns>User removal view.</returns>
    [HttpGet]
    [Route("UserRemoval")]
    public IActionResult UserRemoval(string content = null)
    {
        var model = !string.IsNullOrEmpty(content)
            ? new DefaultEmailData { Content = content }
            : null;

        return View(model);
    }

    /// <summary>
    /// Get join invitation view.
    /// </summary>
    /// <param name="content">Optional content to preview.</param>
    /// <returns>Join invitation view.</returns>
    [HttpGet]
    [Route("JoinInvitation")]
    public IActionResult JoinInvitation(string content = null)
    {
        var model = !string.IsNullOrEmpty(content)
            ? new DefaultEmailData { Content = content }
            : null;

        return View(model);
    }

    /// <summary>
    /// Get join request view.
    /// </summary>
    /// <param name="content">Optional content to preview.</param>
    /// <returns>Join request view.</returns>
    [HttpGet]
    [Route("JoinRequest")]
    public IActionResult JoinRequest(string content = null)
    {
        var model = !string.IsNullOrEmpty(content)
            ? new DefaultEmailData { Content = content }
            : null;

        return View(model);
    }

    /// <summary>
    /// Get pending requests reminder view.
    /// </summary>
    /// <param name="content">Optional content to preview.</param>
    /// <returns>Pending requests reminder view.</returns>
    [HttpGet]
    [Route("PendingRequestsReminder")]
    public IActionResult PendingRequestsReminder(string content = null)
    {
        var model = !string.IsNullOrEmpty(content)
            ? new DefaultEmailData { Content = content }
            : null;

        return View(model);
    }

    /// <summary>
    /// Test email sending with a specific template.
    /// </summary>
    /// <param name="template">Template name (RoleAssignment, UserRemoval, JoinInvitation, JoinRequest, PendingRequestsReminder).</param>
    /// <param name="email">Email address to send test email.</param>
    /// <param name="content">Email content (HTML format).</param>
    /// <param name="subject">Email subject.</param>
    /// <returns>Test result.</returns>
    [HttpPost]
    [Route("Test/{template}")]
    public async Task<IActionResult> TestEmail(string template, string email, string content = null, string subject = null)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest(new { error = "Email address is required" });
        }

        var validTemplates = new[] { "RoleAssignment", "UserRemoval", "JoinInvitation", "JoinRequest", "PendingRequestsReminder" };
        if (!validTemplates.Contains(template))
        {
            return BadRequest(new { error = $"Invalid template. Valid templates: {string.Join(", ", validTemplates)}" });
        }

        var emailData = new DefaultEmailData
        {
            Address = new CustomEmailAddress("Usuario de Prueba", email),
            Subject = subject ?? $"Prueba de plantilla: {template}",
            Content = content ?? $"<p>Este es un correo de prueba para la plantilla <b>{template}</b>.</p>",
        };

        try
        {
            var htmlBody = await webViewTools.RenderViewToStringAsync(template, emailData);

            var receivers = new CustomEmailAddress[] { emailData.Address };
            var response = await emailService.SendEmailAsync(emailData.Subject, receivers, null, htmlBody);

            return Json(new
            {
                success = true,
                message = "Email sent successfully",
                template = template,
                email = email,
                serverResponse = response,
                htmlBodyLength = htmlBody.Length,
            });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, Json(new
            {
                success = false,
                error = ex.Message,
                stackTrace = ex.StackTrace,
                template = template,
                email = email,
            }));
        }
    }
}
