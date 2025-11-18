namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using System;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

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
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.InitiativeId)
            .NotNull()
                .WithMessage("Initiative identifier is required");

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.Level)
                .NotNull()
                    .WithMessage("User level cannot be null")
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
                        .WithMessage("User level must be either 'Member' or 'Reader'.");
                });
        });

        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.Status)
                .NotNull()
                    .WithMessage("Status cannot be null")
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
                        .WithMessage("Invalid selected status");
                });
        });
    }
}
