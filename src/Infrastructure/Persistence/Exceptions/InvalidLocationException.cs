namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Exceptions;

using System;

/// <summary>
/// Invalid location exception.
/// </summary>
public class InvalidLocationException : Exception
{
    /// <inheritdoc/>
    public InvalidLocationException()
    {
    }

    /// <inheritdoc/>
    public InvalidLocationException(string message)
        : base(message)
    {
    }

    /// <inheritdoc/>
    public InvalidLocationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
