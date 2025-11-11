namespace IAVH.BioTablero.CM.Application.Validators.TerritoryStory;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

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
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.FileUrl)
            .NotEmpty()
                .WithMessage("File url is required");

        RuleFor(dto => dto.Description)
            .NotEmpty()
                .WithMessage("Description is required")
            .MaximumLength(500);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.TerritoryStoryId)
                .NotNull()
                    .WithMessage("Territory Story identifier is required");
        });
    }
}
