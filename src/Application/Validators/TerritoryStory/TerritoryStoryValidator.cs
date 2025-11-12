namespace IAVH.BioTablero.CM.Application.Validators.TerritoryStory;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

/// <summary>
/// Territory story validator.
/// </summary>
public class TerritoryStoryValidator : AbstractValidator<TerritoryStoryDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public TerritoryStoryValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.Title)
            .NotEmpty()
                .WithMessage("Title is required")
            .MaximumLength(100);

        RuleFor(dto => dto.Text)
            .NotEmpty()
                .WithMessage("Text is required")
            .MaximumLength(2000);

        RuleFor(dto => dto.Restricted)
            .NotNull()
                .WithMessage("Restricted flag is required");

        RuleFor(dto => dto.Keywords)
            .Matches(RegExprConstants.Keywords)
            .MaximumLength(75);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithMessage("Initiative identifier is required");

            RuleFor(dto => dto.AuthorUserName)
                .NotEmpty()
                    .WithMessage("Author is required")
                .MaximumLength(75);

            RuleForEach(dto => dto.Images)
                .SetValidator(new TerritoryStoryImageValidator(), "default");

            RuleForEach(dto => dto.Videos)
                .SetValidator(new TerritoryStoryVideoValidator(), "default");
        });
    }
}
