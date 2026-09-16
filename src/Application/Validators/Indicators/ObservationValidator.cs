namespace IAVH.BioTablero.CM.Application.Validators.Indicators;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Observation validator.
/// </summary>
public class ObservationValidator : AbstractValidator<ObservationDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public ObservationValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(240)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);
    }
}
