namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Initiative validator.
/// </summary>
public class InitiativeValidator : AbstractValidator<InitiativeDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(100)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.ShortName)
            .MaximumLength(120)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Description)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(300)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Baseline)
            .MaximumLength(1000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Objective)
            .MaximumLength(1000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.Locations)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCodes.Initiatives.LocationsRequired);

            RuleForEach(dto => dto.Locations)
                .SetValidator(new InitiativeLocationValidator(), "default");

            RuleFor(dto => dto.Contacts)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCodes.Initiatives.ContactsRequired);

            RuleForEach(dto => dto.Contacts)
                .SetValidator(new InitiativeContactValidator(), "default");

            RuleFor(dto => dto.Users)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCodes.Initiatives.UsersRequired);

            RuleForEach(dto => dto.Users)
                .SetValidator(new InitiativeUserValidator(), "default");
        });
    }
}
