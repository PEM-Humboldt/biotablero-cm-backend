namespace IAVH.BioTablero.CM.Application.Validators.TerritoryStory;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

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
                .WithMessage("File url is required")
            .MaximumLength(150);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.TerritoryStoryId)
                .NotNull()
                    .WithMessage("Territory Story identifier is required");
        });
    }
}
