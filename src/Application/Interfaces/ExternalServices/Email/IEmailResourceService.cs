namespace IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Email;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

/// <summary>
/// Email service interface for Resource entities.
/// </summary>
public interface IEmailResourceService
{
    /// <summary>
    /// Send notification for resource update.
    /// </summary>
    /// <param name="resource">Resource data.</param>
    /// <param name="userName">Editor user name.</param>
    /// <param name="initiativeUsers">Initiative user names.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    Task<bool> SendNotificationUpdateResource(Resource resource, string userName, string[] initiativeUsers, CancellationToken ct = default);
}
