namespace IAVH.BioTablero.CM.Application.Validators.Indicators;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Topic validator.
/// </summary>
public class TopicValidator : AbstractValidator<TopicDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public TopicValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(500)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);
    }
}
