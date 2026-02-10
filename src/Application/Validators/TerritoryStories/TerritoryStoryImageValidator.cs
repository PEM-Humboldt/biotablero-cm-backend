namespace IAVH.BioTablero.CM.Application.Validators.TerritoryStories;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Territory story image validator.
/// </summary>
public class TerritoryStoryImageValidator : AbstractValidator<TerritoryStoryImageDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public TerritoryStoryImageValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyEntityData);

        RuleFor(dto => dto.Description)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
            .MaximumLength(500);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.TerritoryStoryId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty);
        });
    }
}
