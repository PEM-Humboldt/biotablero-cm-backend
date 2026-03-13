namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

/// <summary>
/// Initiative contact validator.
/// </summary>
public class InitiativeContactValidator : AbstractValidator<InitiativeContactDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeContactValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Email)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .EmailAddress()
                .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue)
            .MaximumLength(100)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Phone)
            .Matches(RegExprConstants.Phone)
                .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue)
            .MinimumLength(7)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength)
            .MaximumLength(15)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);
        });
    }
}
