namespace IAVH.BioTablero.CM.Application.Validators.Indicators;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Indicator validator.
/// </summary>
public class IndicatorValidator : AbstractValidator<IndicatorDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public IndicatorValidator()
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
