namespace IAVH.BioTablero.CM.Application.Validators.TerritoryStories;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
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
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.FileUrl)
            .NotEmpty()
                .WithMessage("{PropertyName} is required")
            .Matches(RegExprConstants.YouTubeVideoUrl)
            .MaximumLength(150);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.TerritoryStoryId)
                .NotNull()
                    .WithMessage("{PropertyName} is required");
        });
    }
}
