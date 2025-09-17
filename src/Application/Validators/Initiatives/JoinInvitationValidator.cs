namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

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
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.Message)
            .MaximumLength(200);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithMessage("Initiative identifier is required");

            RuleFor(dto => dto.Guests)
                .NotEmpty()
                    .WithMessage("At least one guest is required");

            RuleForEach(dto => dto.Guests)
                .SetValidator(new JoinInvitationGuestValidator(), "default");
        });
    }
}
