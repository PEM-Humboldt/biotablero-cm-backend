namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Resource;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

/// <summary>
/// Resource list response example.
/// </summary>
public class ResourceOdataResponseExample : BaseOdataResponseExample<ResourceDto>
{
    /// <inheritdoc/>
    protected override ResourceDto CreateExampleDto() =>
        new()
        {
            Id = 0,
            InitiativeId = 0,
            AuthorUserName = "example@example.com",
            ResourceType = new()
            {
                Id = 0,
                Name = "Resource type example",
            },
            Name = "Resource example",
            Description = "Example",
            IsDraft = false,
            Likes = 0,
            ILikedIt = false,
        };
}
