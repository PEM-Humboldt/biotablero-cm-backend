namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;

using System;

/// <summary>
/// General Storage exception.
/// </summary>
public class StorageException : Exception
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public StorageException()
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public StorageException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Inner exception.</param>
    public StorageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
