namespace IAVH.BioTablero.CM.Application.Services.Users;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Users;
using IAVH.BioTablero.CM.Application.Utils;

/// <summary>
/// User service.
/// </summary>
public class UserService : IUserService
{
    private readonly IIamService iamService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="iamService">IAM service.</param>
    public UserService(
        IIamService iamService)
    {
        this.iamService = iamService;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetAllAsync(CancellationToken ct = default)
    {
        // Get users data
        var users = await iamService.GetAllEnabledUsersDataAsync(ct);

        return new()
        {
            ResponseBody = users,
        };
    }
}
