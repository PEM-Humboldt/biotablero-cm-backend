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
    /// <param name="logType">System log type.</param>
    /// <param name="shortMessage">Short log message.</param>
    /// <param name="messageTemplate">Message template.</param>
    /// <param name="propertyValues">Message property values.</param>
    /// <param name="logLevel">System log level.</param>
    public static void AddLog(this ILogger logger, LogType logType, string shortMessage, string messageTemplate = null, object propertyValues = null, LogEventLevel logLevel = LogEventLevel.Information)
    {
        string finalMessageTemplate;

        if (string.IsNullOrEmpty(shortMessage))
        {
            finalMessageTemplate = messageTemplate;
        }
        else if (string.IsNullOrEmpty(messageTemplate))
        {
            finalMessageTemplate = shortMessage;
        }
        else
        {
            finalMessageTemplate = $"{shortMessage}: {messageTemplate}";
        }

        logger
            .ForContext("CustomRecord", true)
            .ForContext("Type", (int)logType)
            .ForContext("ShortMessage", shortMessage)
            .Write(logLevel, finalMessageTemplate, propertyValues);
    }
}
