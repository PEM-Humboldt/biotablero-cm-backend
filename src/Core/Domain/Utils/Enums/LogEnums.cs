namespace IAVH.BioTablero.CM.Core.Domain.Utils.Enums;

/// <summary>
/// Logs enumerations.
/// </summary>
public static class LogEnums
{
    /// <summary>
    /// Serilog log levels.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Level for extremely detailed tracing.
        /// </summary>
        Verbose,

        /// <summary>
        /// Level for debugging purposes.
        /// </summary>
        Debug,

        /// <summary>
        /// Level for information about application operations and events.
        /// </summary>
        Information,

        /// <summary>
        /// Level for potential issues or unexpected behavior that may require attention.
        /// </summary>
        Warning,

        /// <summary>
        /// Level for errors that have occurred within the application.
        /// </summary>
        Error,

        /// <summary>
        /// Level for critical errors that cause the application to terminate.
        /// </summary>
        Fatal,
    }

    /// <summary>
    /// Log system type.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// Internal system event.
        /// </summary>
        System = 1,

        /// <summary>
        /// Create event.
        /// </summary>
        Create,

        /// <summary>
        /// Read event.
        /// </summary>
        Read,

        /// <summary>
        /// Update event.
        /// </summary>
        Update,

        /// <summary>
        /// Delete event.
        /// </summary>
        Delete,
    }
}
