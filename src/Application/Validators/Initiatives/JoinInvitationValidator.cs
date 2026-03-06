namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Join invitation validator.
/// </summary>
public class JoinInvitationValidator : AbstractValidator<JoinInvitationDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public JoinInvitationValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Creator)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(75);

        RuleFor(dto => dto.Message)
            .MaximumLength(350);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);

            RuleFor(dto => dto.Guests)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCodes.JoinInvitations.GuestsRequired);

            RuleForEach(dto => dto.Guests)
                .SetValidator(new JoinInvitationGuestValidator(), "default");
        });
    }
}
