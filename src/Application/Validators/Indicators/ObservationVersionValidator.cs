namespace IAVH.BioTablero.CM.Application.Validators.Indicators;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Observation Version validator.
/// </summary>
public class ObservationVersionValidator : AbstractValidator<ObservationVersionDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public ObservationVersionValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Description)
            .MaximumLength(3000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Methodology)
            .MaximumLength(3000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Interpretation)
            .MaximumLength(3000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Considerations)
            .MaximumLength(3000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Authorship)
            .MaximumLength(3000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);
    }
}
