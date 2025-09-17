namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

/// <summary>
/// Join invitation guest validator.
/// </summary>
public class JoinInvitationGuestValidator : AbstractValidator<JoinInvitationGuestDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public JoinInvitationGuestValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.Email)
            .NotEmpty()
                .WithMessage("Email is required")
            .EmailAddress()
            .MaximumLength(100);
    }
}
