namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

/// <summary>
/// Initiative user validator
/// </summary>
public class InitiativeUserValidator : AbstractValidator<InitiativeUserDto>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public InitiativeUserValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.Level)
            .NotNull()
                .WithMessage("Level cannot be null")
            .ChildRules(level =>
            {
                level.RuleFor(levelEnumDto => levelEnumDto.Id)
                    .IsInEnum()
                        .WithMessage("Level id invalid");
            });
    }
}
