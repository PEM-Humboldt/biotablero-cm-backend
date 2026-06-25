namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.TagEnums;

/// <summary>
/// Initiative geo data response example.
/// </summary>
public class InitiativeGeoDataResponseExample : IExamplesProvider<List<InitiativeDto>>
{
    /// <inheritdoc/>
    public List<InitiativeDto> GetExamples() => [
        new()
        {
            Id = 1,
            Name = "Initiative example",
            CreationDate = DateTime.Now,
            Coordinate = [-74.09423914807002, 4.645238678888821],
            Tags = [
                new()
                {
                    InitiativeTagId = 0,
                    Tag = new()
                    {
                        Id = 0,
                        Name = "Tag example",
                        Url = "https://example.com/tag-data",
                        Category = new EnumEntityDto<TagCategory>(TagCategory.PoliticalContext),
                    },
                },
            ],
        },
    ];
}
