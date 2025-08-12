namespace IAVH.BioTablero.CM.Application.Utils;

using Serilog;
using Serilog.Events;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Custom Log utils.
/// </summary>
public static class LogUtils
{
    /// <summary>
    /// Add custom log message.
    /// </summary>
    /// <param name="logger">Serilog logger.</param>
    /// <param name="messageTemplate">Message template.</param>
    /// <param name="propertyValues">Message property values.</param>
    /// <param name="logType">System log type.</param>
    /// <param name="logLevel">System log level.</param>
    public static void Add(this ILogger logger, string messageTemplate, object propertyValues, LogType logType = LogType.System, LogEventLevel logLevel = LogEventLevel.Information) =>
        logger
            .ForContext("CustomRecord", true)
            .ForContext("Type", (int)logType)
            .Write(logLevel, messageTemplate, propertyValues);
}
