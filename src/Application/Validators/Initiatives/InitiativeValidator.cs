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
            .MaximumLength(100);

        RuleFor(dto => dto.ShortName)
            .MaximumLength(120);

        RuleFor(dto => dto.Description)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(300);

        RuleFor(dto => dto.Baseline)
            .MaximumLength(1000);

        RuleFor(dto => dto.Objective)
            .MaximumLength(1000);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.Locations)
                .NotEmpty()
                    .WithMessage("At least one location is required");

            RuleForEach(dto => dto.Locations)
                .SetValidator(new InitiativeLocationValidator(), "default");

            RuleFor(dto => dto.Contacts)
                .NotEmpty()
                    .WithMessage("At least one contact is required");

            RuleForEach(dto => dto.Contacts)
                .SetValidator(new InitiativeContactValidator(), "default");

            RuleFor(dto => dto.Users)
                .NotEmpty()
                    .WithMessage("At least one user is required");

            RuleForEach(dto => dto.Users)
                .SetValidator(new InitiativeUserValidator(), "default");
        });
    }
}
