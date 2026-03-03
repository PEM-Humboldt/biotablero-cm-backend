namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative user validator.
/// </summary>
public class InitiativeUserValidator : AbstractValidator<InitiativeUserDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeUserValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.FocusArea)
            .MaximumLength(200);

        RuleFor(dto => dto.Level)
            .NotNull()
                .WithMessage("{PropertyName} cannot be null")
            .ChildRules(level =>
            {
                level.RuleFor(levelEnumDto => levelEnumDto.Name)
                    .IsEnumName(typeof(InitiativeUserLevel), caseSensitive: false)
                        .WithMessage("{PropertyName} invalid");
            });

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithMessage("{PropertyName} is required");

            RuleFor(dto => dto.UserName)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required")
                .MaximumLength(75);
        });
    }
}
