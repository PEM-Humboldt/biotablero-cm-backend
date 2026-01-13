namespace IAVH.BioTablero.CM.Application.Services.Users;

using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Users;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

/// <summary>
/// User service.
/// </summary>
public class UserService : IUserService
{
    private readonly IIamService iamService;
    private readonly IInitiativeUserRepository initiativeUserRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="iamService">IAM service.</param>
    /// <param name="initiativeUserRepository">Initiative User repository.</param>
    public UserService(
        IIamService iamService,
        IInitiativeUserRepository initiativeUserRepository)
    {
        this.iamService = iamService;
        this.initiativeUserRepository = initiativeUserRepository;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetAllAsync(string userName, string[] roles, CancellationToken ct = default)
    {
        // Validate users permissions
        var userIsAdmin = roles.Contains(IamConstants.RoleModuleAdmin);
        var authorizedUserAction = await initiativeUserRepository.AuthorizedEntityModifyAsync(userName, ct);

        if (!(userIsAdmin || authorizedUserAction))
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Get users data
        var users = await iamService.GetAllEnabledUsersDataAsync(ct);

        return new()
        {
            ResponseBody = users,
        };
    }
}
