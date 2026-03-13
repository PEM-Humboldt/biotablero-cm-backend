namespace IAVH.BioTablero.CM.Application.Validators.TerritoryStories;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

/// <summary>
/// Territory story video validator.
/// </summary>
public class TerritoryStoryVideoValidator : AbstractValidator<TerritoryStoryVideoDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public TerritoryStoryVideoValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.FileUrl)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .Matches(RegExprConstants.YouTubeVideoUrl)
                .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue)
            .MaximumLength(150)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.TerritoryStoryId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);
        });
    }
}
