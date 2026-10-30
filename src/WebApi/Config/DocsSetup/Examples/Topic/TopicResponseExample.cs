namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Topic;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Topic response example.
/// </summary>
public class TopicResponseExample : IOpenApiExampleProvider<TopicDto>
{
    /// <inheritdoc/>
    public TopicDto GetExamples() => new()
    {
        Id = 0,
        Name = "Topic example",
        InitiativeId = 0,
        TopicType = new()
        {
            Id = 0,
            Name = "Topic type example",
        },
        Versions = [
            new()
            {
                Id = 0,
                Version = 0,
                CreationDate = DateTime.Now,
            }
        ],
        Locations = [
            new()
            {
                Id = 0,
                LocationId = 0,
                Locality = "Locality example",
                Location = new()
                {
                    Id = 0,
                    Name = "Example",
                    Code = "000",
                    Parent = new()
                    {
                        Id = 0,
                        Name = "Example",
                        Code = "000",
                    },
                },
            },
        ],
        Tags = [
            new()
            {
                TopicTagId = 0,
                Tag = new()
                {
                    Id = 0,
                    Name = "Tag example",
                },
            }
        ],
    };
}
