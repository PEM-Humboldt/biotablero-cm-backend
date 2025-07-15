namespace IAVH.BioTablero.CM.Core.Enums;

public static class LogEnums
{
    public enum LogLevel
    {
        Verbose,
        Debug,
        Information,
        Warning,
        Error,
        Fatal,
    }

    public enum LogType : int
    {
        Create = 1,
        Read,
        Update,
        Delete,
    }
}
