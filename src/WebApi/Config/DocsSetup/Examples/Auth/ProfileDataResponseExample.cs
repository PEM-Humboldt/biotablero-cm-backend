namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Auth;

using IAVH.BioTablero.CM.Core.Domain.Models.User;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// User profile data response example.
/// </summary>
public class ProfileDataResponseExample : IOpenApiExampleProvider<UserProfile>
{
    /// <inheritdoc/>
    public UserProfile GetExamples() => new()
    {
        Username = "initiative-user@example.com",
    };
}
