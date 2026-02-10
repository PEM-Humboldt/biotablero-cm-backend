namespace IAVH.BioTablero.CM.Application.Validators.TerritoryStories;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
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
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Title)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(100);

        RuleFor(dto => dto.Text)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(2000);

        RuleFor(dto => dto.Restricted)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);

        RuleFor(dto => dto.Keywords)
            .Matches(RegExprConstants.Keywords)
                .WithErrorCode(ValidationErrorCodes.TerritoryStories.InvalidKeywords)
            .MaximumLength(75);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);

            RuleFor(dto => dto.AuthorUserName)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
                .MaximumLength(75);

            RuleForEach(dto => dto.Videos)
                .SetValidator(new TerritoryStoryVideoValidator(), "default");
        });
    }
}
