namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Auth;

using IAVH.BioTablero.CM.Core.Domain.Models.User;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// User profile data response example.
/// </summary>
public class ProfileDataResponseExample : IExamplesProvider<UserProfile>
{
    /// <inheritdoc/>
    public UserProfile GetExamples() => new()
    {
        Username = "initiative-user@example.com",
    };
}
