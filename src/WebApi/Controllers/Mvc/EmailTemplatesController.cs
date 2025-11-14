namespace IAVH.BioTablero.CM.WebApi.Controllers.Mvc;

using IAVH.BioTablero.CM.Core.Domain.Utils.Email;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Email Templates controller.
/// </summary>
[Route("Email")]
[ApiExplorerSettings(IgnoreApi = true)]
public class EmailTemplatesController : Controller
{
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
            ? new RoleAssignmentEmailData { Content = content }
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
            ? new UserRemovalEmailData { Content = content }
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
            ? new JoinInvitationEmailData { Content = content }
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
            ? new JoinRequestEmailData { Content = content }
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
            ? new PendingRequestsReminderEmailData { Content = content }
            : null;

        return View(model);
    }
}
