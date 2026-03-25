namespace IAVH.BioTablero.CM.WebApi.Utils;

using System.Linq;
using System.Security.Claims;

using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

using Microsoft.AspNetCore.Http;

/// <summary>
/// General HTTP context extensions.
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Get username.
    /// </summary>
    /// <param name="httpContext">Current HTTP Context.</param>
    /// <returns>User name.</returns>
    public static string GetUserName(this HttpContext httpContext)
    {
        var username = httpContext?.User.FindAll(IamConstants.UserName);
        return username.FirstOrDefault()?.Value;
    }

    /// <summary>
    /// Get user roles.
    /// </summary>
    /// <param name="httpContext">Current HTTP Context.</param>
    /// <returns>User roles.</returns>
    public static string[] GetRoles(this HttpContext httpContext) =>
        httpContext?.User
            .FindAll(ClaimTypes.Role)
            .Select(r => r.Value)
            .ToArray();

    /// <summary>
    /// Checks if the current user is an administrator.
    /// </summary>
    /// <param name="httpContext">Current HTTP Context.</param>
    /// <returns>True if the user is an adminstrator. False otherwise.</returns>
    public static bool UserIsAdmin(this HttpContext httpContext) =>
        GetRoles(httpContext).Contains(IamConstants.RoleModuleAdmin);
}
