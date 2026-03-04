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
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Description)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(500)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.TerritoryStoryId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);
        });
    }
}
