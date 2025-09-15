namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Config.Entities;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Config.General;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Interfaces;

/// <summary>
/// Log report configuration.
/// </summary>
public class LogReportConfig : IReportConfig<LogDto>
{
    /// <summary>
    /// Configure entity.
    /// </summary>
    /// <param name="builder">Entity builder.</param>
    public void Configure(ReportMapBuilder<LogDto> builder)
    {
        builder
            .Property(x => x.Id, index: 1, visible: true, header: "Identificador");

        builder
            .Property(x => x.TimeStamp, index: 2, visible: true, header: "Hora y fecha");

        builder
            .Property(x => x.Type, index: 3, visible: true, header: "Tipo");

        builder
            .Property(x => x.UserName, index: 4, visible: true, header: "Usuario");

        builder
            .Property(x => x.ClientIp, index: 5, visible: true, header: "Dirección IP");

        builder
            .Property(x => x.ClientAgent, index: 6, visible: true, header: "Navegador");

        builder
            .Property(x => x.Message, index: 7, visible: true, header: "Descripción");
    }
}
