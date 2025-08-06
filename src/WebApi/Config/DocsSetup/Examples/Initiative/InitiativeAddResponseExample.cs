namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative Add response example.
/// </summary>
public class InitiativeAddResponseExample : IExamplesProvider<InitiativeDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public InitiativeDto GetExamples() => new()
    {
        Name = "Initiative example",
        Description = "example",
        InitiativeLocations = [
            new()
            {
                Location = new() { Id = 1 },
                Locality = "Locality example",
            }
        ],
        InitiativeContacts = [
            new()
            {
                Phone = "3055555555",
                Email = "example@example.com",
            }
        ],
        InitiativeUsers = [
            new()
            {
                UserName = "Admin",
                Level = new EnumEntityDto<InitiativeUserLevel>(InitiativeUserLevel.Leader),
            }
        ],
    };
}
