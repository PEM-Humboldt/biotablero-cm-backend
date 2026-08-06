namespace IAVH.BioTablero.CM.Application.Validators.Reports;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Report Data validator.
/// </summary>
public class ReportDataValidator : AbstractValidator<ReportDataDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public ReportDataValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Description)
            .MaximumLength(500)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Data)
            .MaximumLength(280)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);
    }
}
