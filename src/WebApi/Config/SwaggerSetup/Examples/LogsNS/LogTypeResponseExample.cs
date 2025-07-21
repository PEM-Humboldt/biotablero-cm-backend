namespace IAVH.BioTablero.CM.WebApi.Config.SwaggerSetup.Examples.LogsNS;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Helpers.General;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Enums.LogEnums;

/// <summary>
/// Log type response example
/// </summary>
/// <param name="entityService">General enumeration service</param>
public class LogTypeResponseExample(IServiceReadEnumeration<LogType> entityService) : IExamplesProvider<IEnumerable<EnumEntityDto<LogType>>>
{
    /// <summary>
    /// Get examples for entity
    /// </summary>
    /// <returns>Entity examples</returns>
    public IEnumerable<EnumEntityDto<LogType>> GetExamples() => entityService.GetEnumerable();
}
