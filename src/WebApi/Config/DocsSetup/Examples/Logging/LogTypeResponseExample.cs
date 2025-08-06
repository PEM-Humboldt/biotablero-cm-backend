namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Logging;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Log type response example.
/// </summary>
/// <param name="entityService">General enumeration service.</param>
public class LogTypeResponseExample(IServiceReadEnumeration<LogType> entityService) : IExamplesProvider<IEnumerable<EnumEntityDto<LogType>>>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public IEnumerable<EnumEntityDto<LogType>> GetExamples() => entityService.GetEnumerable();
}
