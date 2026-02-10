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
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyEntityData);

        RuleFor(dto => dto.Title)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
            .MaximumLength(100);

        RuleFor(dto => dto.Text)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
            .MaximumLength(2000);

        RuleFor(dto => dto.Restricted)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty);

        RuleFor(dto => dto.Keywords)
            .Matches(RegExprConstants.Keywords)
                .WithMessage("Invalid format. Keywords must be separated by commas. Spaces and special characters are not allowed. Only up to 5 keywords are allowed.")
            .MaximumLength(75);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty);

            RuleFor(dto => dto.AuthorUserName)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
                .MaximumLength(75);

            RuleForEach(dto => dto.Videos)
                .SetValidator(new TerritoryStoryVideoValidator(), "default");
        });
    }
}
