namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeContact;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Initiative Contact list response example.
/// </summary>
public class InitiativeContactListResponseExample : IOpenApiExampleProvider<List<InitiativeContactDto>>
{
    /// <inheritdoc/>
    public List<InitiativeContactDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            InitiativeId = 0,
            Phone = "3055555555",
            Email = "example@example.com",
        },
    ];
}
