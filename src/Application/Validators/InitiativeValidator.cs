namespace IAVH.BioTablero.CM.Application.Validators;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

/// <summary>
/// Initiative validator
/// </summary>
public class InitiativeValidator : AbstractValidator<InitiativeDto>
{
    /// <summary>
    /// Constructor
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
            .NotNull()
            .ChildRules(o =>
            {
                o.RuleFor(o => o.Locality)
                    .MaximumLength(300);
            });

        RuleFor(dto => dto.InitiativeContacts)
            .NotEmpty()
                .WithMessage("At least one contact is required");

        RuleForEach(x => x.InitiativeContacts)
            .NotNull()
            .ChildRules(o =>
            {
                o.RuleFor(o => o.Phone)
                    .MaximumLength(15);

                o.RuleFor(o => o.Email)
                    .MaximumLength(100);
            });

        RuleFor(dto => dto.InitiativeUsers)
            .NotEmpty()
                .WithMessage("At least one user is required");

        RuleForEach(x => x.InitiativeUsers)
            .NotNull()
            .ChildRules(o =>
            {
                o.RuleFor(o => o.Level)
                    .NotNull()
                        .WithMessage("Level cannot be null")
                    .ChildRules(o =>
                    {
                        o.RuleFor(o => o.Id)
                            .IsInEnum()
                                .WithMessage("Level id invalid");
                    });
            });
    }
}
