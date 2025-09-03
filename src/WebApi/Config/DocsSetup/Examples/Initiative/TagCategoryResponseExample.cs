namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Tag Category response example.
/// </summary>
/// <param name="entityService">General enumeration service.</param>
public class TagCategoryResponseExample(IReadEnumeration<TagCategory> entityService) : IExamplesProvider<IEnumerable<EnumEntityDto<TagCategory>>>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public IEnumerable<EnumEntityDto<TagCategory>> GetExamples() => entityService.GetEnumerable();
}
