namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System.Collections.Generic;

using FluentValidation.Results;

using IAVH.BioTablero.CM.Application.Domain;

/// <summary>
/// Validation error translator interface.
/// </summary>
public interface IValidationErrorTranslator
{
    /// <summary>
    /// Translate error message.
    /// </summary>
    /// <param name="errorCode">Error code.</param>
    /// <param name="propertyName">Property name.</param>
    /// <returns>Translated message.</returns>
    ApiValidationError Translate(string errorCode, string propertyName = null);

    /// <summary>
    /// Translate error messages.
    /// </summary>
    /// <param name="failures">Failures list.</param>
    /// <returns>Translated messages.</returns>
    IEnumerable<ApiValidationError> Translate(
        IEnumerable<ValidationFailure> failures);
}
