namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

/// <summary>
/// Initiative contact validator
/// </summary>
public class InitiativeContactValidator : AbstractValidator<InitiativeContactDto>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public InitiativeContactValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithMessage("Entity data cannot be null");

        RuleFor(o => o.Email)
            .NotEmpty()
                .WithMessage("Email is required")
            .MaximumLength(100);

        RuleFor(o => o.Phone)
            .MaximumLength(15);
    }
}
