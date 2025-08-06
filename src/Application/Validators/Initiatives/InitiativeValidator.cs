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

        RuleFor(dto => dto.InitiativeLocations)
            .NotEmpty()
                .WithMessage("At least one location is required");

        RuleForEach(x => x.InitiativeLocations)
            .SetValidator(new InitiativeLocationValidator());

        RuleFor(dto => dto.InitiativeContacts)
            .NotEmpty()
                .WithMessage("At least one contact is required");

        RuleForEach(x => x.InitiativeContacts)
            .SetValidator(new InitiativeContactValidator());

        RuleFor(dto => dto.InitiativeUsers)
            .NotEmpty()
                .WithMessage("At least one user is required");

        RuleForEach(x => x.InitiativeUsers)
            .SetValidator(new InitiativeUserValidator());
    }
}
