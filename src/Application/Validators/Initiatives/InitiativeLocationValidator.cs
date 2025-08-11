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

        RuleFor(dto => dto.LocationId)
            .NotNull()
                .WithMessage("Location identifier is required");

        RuleFor(dto => dto.Locality)
            .MaximumLength(300);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithMessage("Initiative identifier is required");
        });
    }
}
