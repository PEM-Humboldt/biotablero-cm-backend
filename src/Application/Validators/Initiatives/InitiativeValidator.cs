namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

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
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithMessage("Name is required")
            .MaximumLength(100);

        RuleFor(dto => dto.Description)
            .NotEmpty()
                .WithMessage("Description is required")
            .MaximumLength(300);

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
