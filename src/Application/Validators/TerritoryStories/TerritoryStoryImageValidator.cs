namespace IAVH.BioTablero.CM.Application.Validators.TerritoryStories;

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

        RuleFor(dto => dto.Description)
            .NotEmpty()
                .WithMessage("{PropertyName} is required")
            .MaximumLength(500);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.TerritoryStoryId)
                .NotNull()
                    .WithMessage("{PropertyName} is required");
        });
    }
}
