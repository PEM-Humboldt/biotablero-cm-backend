namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Email;

using System;

/// <summary>
/// Email exception.
/// </summary>
public class EmailException : Exception
{
    /// <inheritdoc/>
    public EmailException()
    {
    }

    /// <inheritdoc/>
    public EmailException(string message)
        : base(message)
    {
    }

    /// <inheritdoc/>
    public EmailException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
