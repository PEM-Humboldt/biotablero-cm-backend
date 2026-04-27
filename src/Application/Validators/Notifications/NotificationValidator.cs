namespace IAVH.BioTablero.CM.Application.Validators.Notifications;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Notifications;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Notification validator.
/// </summary>
public class NotificationValidator : AbstractValidator<NotificationDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public NotificationValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Receiver)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(75)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Subject)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(120)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Body)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(2000)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);
    }
}
