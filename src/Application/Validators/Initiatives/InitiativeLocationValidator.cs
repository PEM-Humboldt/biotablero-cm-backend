namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

/// <summary>
/// Initiative location validator.
/// </summary>
public class InitiativeLocationValidator : AbstractValidator<InitiativeLocationDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeLocationValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithMessage("Entity data cannot be null");

        RuleFor(o => o.Location)
            .NotNull()
                .WithMessage("Location cannot be null");

        RuleFor(o => o.Locality)
            .MaximumLength(300);
    }
}
