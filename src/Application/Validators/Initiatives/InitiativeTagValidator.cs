namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative Tag validator.
/// </summary>
public class InitiativeTagValidator : AbstractValidator<InitiativeTagDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeTagValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithMessage("Name is required")
            .MaximumLength(40);

        RuleFor(dto => dto.Url)
            .Must(uri => uri == null || uri.ToString().Length <= 150)
                .WithMessage("'{PropertyName}' cannot exceed 150 characters");

        RuleFor(dto => dto.Category)
            .NotNull()
                .WithMessage("Tag category cannot be null")
            .ChildRules(level =>
            {
                level.RuleFor(tagEnumDto => tagEnumDto.Name)
                    .IsEnumName(typeof(InitiativeTagCategory), caseSensitive: false)
                        .WithMessage("Tag category invalid");
            });
    }
}
