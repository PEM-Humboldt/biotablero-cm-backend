namespace IAVH.BioTablero.CM.Application.Validators.Indicators;

using FluentValidation;

using IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

/// <summary>
/// Indicators Import Row validator.
/// </summary>
public class IndicatorsImportRowValidator : AbstractValidator<IndicatorsImportRow>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public IndicatorsImportRowValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.DepartmentName)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData)
            .MaximumLength(80);

        RuleFor(dto => dto.MunicipalityName)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData)
            .MaximumLength(80)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.LocalityName)
            .MaximumLength(300)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Year)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData)
            .MaximumLength(4)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength)
            .Matches(RegExprConstants.Year)
                .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue);

        RuleFor(dto => dto.Month)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData)
            .MaximumLength(2)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength)
            .Matches(RegExprConstants.Month)
                .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue);

        When(dto => !string.IsNullOrEmpty(dto.FinalYear), () =>
        {
            RuleFor(dto => dto.FinalYear)
                .MaximumLength(4)
                    .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength)
                .Matches(RegExprConstants.Year)
                    .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue);
        });

        When(dto => !string.IsNullOrEmpty(dto.FinalMonth), () =>
        {
            RuleFor(dto => dto.FinalMonth)
                .MaximumLength(2)
                    .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength)
                .Matches(RegExprConstants.Month)
                    .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue);
        });

        RuleFor(dto => dto.UpperGroupName)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData)
            .MaximumLength(70)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.GroupName)
            .MaximumLength(70)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.GroupDescription)
            .MaximumLength(240)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);
    }
}
