namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;

using System;

/// <summary>
/// General Storage exception.
/// </summary>
public class StorageException : Exception
{
    /// <inheritdoc/>
    public StorageException()
    {
    }

    /// <inheritdoc/>
    public StorageException(string message)
        : base(message)
    {
    }

    /// <inheritdoc/>
    public StorageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
