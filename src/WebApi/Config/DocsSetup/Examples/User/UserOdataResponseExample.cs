namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.User;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// User OData response example.
/// </summary>
public class UserOdataResponseExample : IExamplesProvider<Dictionary<string, object>>
{
    /// <inheritdoc/>
    public Dictionary<string, object> GetExamples() => new()
    {
        ["@odata.count"] = 1,
        ["value"] = new List<ExternalUser>()
        {
            new()
            {
                Id = Guid.Empty,
                Email = "user@example.com",
                EmailVerified = false,
                Username = "user@example.com",
                FirstName = "User",
                LastName = "Example",
            },
        },
    };
}
