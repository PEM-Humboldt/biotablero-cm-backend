namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative add request example.
/// </summary>
public class InitiativeAddRequestExample : IExamplesProvider<InitiativeDto>
{
    /// <inheritdoc/>
    public InitiativeDto GetExamples() => new()
    {
        Name = "Initiative example",
        ShortName = "IE",
        Description = "example",
        Baseline = "Baseline example",
        Objective = "Objective example",
        Locations = [
            new()
            {
                LocationId = 1,
            }
        ],
        Contacts = [
            new()
            {
                Phone = "3055555555",
                Email = "example@example.com",
            }
        ],
        Users = [
            new()
            {
                UserName = "general-admin",
                Level = new EnumEntityDto<InitiativeUserLevel>(InitiativeUserLevel.Leader),
            }
        ],
    };
}
