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
    /// <param name="failures">Failures list.</param>
    /// <returns>Translated messages.</returns>
    IEnumerable<ApiValidationError> Translate(
        IEnumerable<ValidationFailure> failures);
}
