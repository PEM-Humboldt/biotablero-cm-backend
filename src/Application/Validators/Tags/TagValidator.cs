namespace IAVH.BioTablero.CM.Application.Validators.Tags;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.TagEnums;

/// <summary>
/// Tag validator.
/// </summary>
public class TagValidator : AbstractValidator<TagDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public TagValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithMessage("Name is required")
            .MaximumLength(40);

        RuleFor(dto => dto.Url)
            .Matches(RegExprConstants.Url)
            .MaximumLength(150);

        RuleFor(dto => dto.Category)
            .NotNull()
                .WithMessage("Tag category cannot be null")
            .ChildRules(level =>
            {
                level.RuleFor(tagEnumDto => tagEnumDto.Name)
                    .IsEnumName(typeof(TagCategory), caseSensitive: false)
                        .WithMessage("Tag category invalid");
            });
    }
}
