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
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyEntityData);

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
            .MaximumLength(40);

        RuleFor(dto => dto.Url)
            .Matches(RegExprConstants.Url)
            .MaximumLength(150);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.Category)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
                .ChildRules(level =>
                {
                    level.RuleFor(tagEnumDto => tagEnumDto.Name)
                        .IsEnumName(typeof(TagCategory), caseSensitive: false)
                            .WithErrorCode(ValidationErrorCodes.GeneralInvalidPropertyValue);
                });
        });
    }
}
