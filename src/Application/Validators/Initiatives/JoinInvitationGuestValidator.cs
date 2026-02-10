namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

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
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyEntityData);

        RuleFor(dto => dto.Email)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
            .EmailAddress()
            .MaximumLength(100);
    }
}
