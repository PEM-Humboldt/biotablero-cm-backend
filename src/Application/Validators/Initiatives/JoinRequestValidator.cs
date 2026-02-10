namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using System;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Join request validator.
/// </summary>
public class JoinRequestValidator : AbstractValidator<JoinRequestDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public JoinRequestValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.InitiativeId)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.Level)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
                .ChildRules(level =>
                {
                    level.RuleFor(levelEnumDto => levelEnumDto.Name)
                        .Must(levelName =>
                        {
                            if (Enum.TryParse(levelName, true, out InitiativeUserLevel userLevel))
                            {
                                return userLevel is InitiativeUserLevel.Member or InitiativeUserLevel.Reader;
                            }

                            return false;
                        })
                        .WithErrorCode(ValidationErrorCodes.JoinRequests.InvalidUserLevel);
                });
        });

        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.Status)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
                .ChildRules(status =>
                {
                    status.RuleFor(statusEnumDto => statusEnumDto.Name)
                        .Must(statusName =>
                        {
                            if (Enum.TryParse(statusName, true, out JoinRequestStatus statusEnum))
                            {
                                return statusEnum is JoinRequestStatus.Approved or JoinRequestStatus.Rejected;
                            }

                            return false;
                        })
                        .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue);
                });
        });
    }
}
