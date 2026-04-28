namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Resource;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.TagEnums;

/// <summary>
/// Resource response example.
/// </summary>
public class ResourceResponseExample : IExamplesProvider<ResourceDto>
{
    /// <inheritdoc/>
    public ResourceDto GetExamples() => new()
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
        Files = [
            new()
            {
                Id = 0,
                ResourceId = 0,
                Name = "Resource link example",
                Url = new Uri("https://cdn.prod.website-files.com/64bea006689ebc2d5d9499ad/67ac933f9b2b1323a3e61073_30-humboldt-negro.svg"),
            }
        ],
        Links = [
            new()
            {
                Id = 0,
                ResourceId = 0,
                Name = "Resource link example",
                Url = "http://example.com",
            }
        ],
        Tags = [
            new()
            {
                ResourceTagId = 0,
                Tag = new()
                {
                    Id = 0,
                    Name = "Tag example",
                    Url = "https://example.com/tag-data",
                    Category = new EnumEntityDto<TagCategory>(TagCategory.PoliticalContext),
                },
            }
        ],
    };
}
