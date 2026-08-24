namespace IAVH.BioTablero.CM.Application.Validators.Indicators;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Indicator Version validator.
/// </summary>
public class IndicatorVersionValidator : AbstractValidator<IndicatorVersionDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public IndicatorVersionValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Description)
            .MaximumLength(1000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Methodology)
            .MaximumLength(1000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Interpretation)
            .MaximumLength(1000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Considerations)
            .MaximumLength(1000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Authorship)
            .MaximumLength(1000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);
    }
}
