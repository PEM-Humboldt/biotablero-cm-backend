namespace IAVH.BioTablero.CM.Application.Validators.Tags;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
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
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(40);

        RuleFor(dto => dto.Url)
            .Matches(RegExprConstants.Url)
                .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue)
            .MaximumLength(150);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.Category)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
                .ChildRules(level =>
                {
                    level.RuleFor(tagEnumDto => tagEnumDto.Name)
                        .IsEnumName(typeof(TagCategory), caseSensitive: false)
                            .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue);
                });
        });
    }
}
