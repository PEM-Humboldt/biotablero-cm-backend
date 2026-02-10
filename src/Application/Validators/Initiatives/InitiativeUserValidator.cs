namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative user validator.
/// </summary>
public class InitiativeUserValidator : AbstractValidator<InitiativeUserDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeUserValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyEntityData);

        RuleFor(dto => dto.FocusArea)
            .MaximumLength(200);

        RuleFor(dto => dto.Level)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
            .ChildRules(level =>
            {
                level.RuleFor(levelEnumDto => levelEnumDto.Name)
                    .IsEnumName(typeof(InitiativeUserLevel), caseSensitive: false)
                        .WithErrorCode(ValidationErrorCodes.GeneralInvalidPropertyValue);
            });

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty);

            RuleFor(dto => dto.UserName)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
                .MaximumLength(75);
        });
    }
}
