namespace IAVH.BioTablero.CM.WebApi.Config.SwaggerSetup.Examples.General;

using System;
using System.Collections.Generic;
using System.Linq;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Helpers.General;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Enums.LogEnums;

public class EnumerationResponseExample : IExamplesProvider<IEnumerable<EnumEntityDto<LogType>>>
{
    public EnumerationResponseExample(IServiceReadEnumeration<LogType> entityService)
    {
        _entityService = entityService;
    }

    private readonly IServiceReadEnumeration<LogType> _entityService;

    public IEnumerable<EnumEntityDto<LogType>> GetExamples() => _entityService.GetEnumerable();
}
