namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

/// <summary>
/// Initiative contact validator.
/// </summary>
public class InitiativeContactValidator : AbstractValidator<InitiativeContactDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeContactValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.Email)
            .NotEmpty()
                .WithMessage("{PropertyName} is required")
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(dto => dto.Phone)
            .Matches(RegExprConstants.Phone)
            .MinimumLength(7)
            .MaximumLength(15);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithMessage("{PropertyName} is required");
        });
    }
}
