namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Config.Entities;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Config.General;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Reports.Interfaces;

public class LogReportConfig : IReportConfig<LogDto>
{
    public void Configure(ReportMapBuilder<LogDto> builder) => builder
        .Property(x => x.Id, "Identificador", index: 1, visible: true)
        .Property(x => x.TimeStamp, "Hora y fecha", index: 2, visible: true)
        .Property(x => x.Type, "Tipo", index: 3, visible: true)
        .Property(x => x.UserName, "Usuario", index: 4, visible: true)
        .Property(x => x.ClientIp, "Dirección IP", index: 5, visible: true)
        .Property(x => x.ClientAgent, "Navegador", index: 6, visible: true)
        .Property(x => x.Message, "Descripción", index: 7, visible: true);
}
