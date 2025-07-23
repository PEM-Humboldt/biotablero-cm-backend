namespace IAVH.BioTablero.CM.WebApi.Utils;

using System.Linq;

using IAVH.BioTablero.CM.Core.Domain.Constants;

using Microsoft.AspNetCore.Http;

/// <summary>
/// General HTTP context extensions
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Get username
    /// </summary>
    /// <param name="httpContext">Current HTTP Context</param>
    /// <returns>User name</returns>
    public static string GetUserName(this HttpContext httpContext)
    {
        var username = httpContext?.User.FindAll(IamConstants.UserName);

        return username.FirstOrDefault()?.Value;
    }
}
