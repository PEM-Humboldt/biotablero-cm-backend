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
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.FocusArea)
            .MaximumLength(200)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Level)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .ChildRules(level =>
            {
                level.RuleFor(levelEnumDto => levelEnumDto.Name)
                    .IsEnumName(typeof(InitiativeUserLevel), caseSensitive: false)
                        .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue);
            });

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);

            RuleFor(dto => dto.UserName)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
                .MaximumLength(75)
                    .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);
        });
    }
}
