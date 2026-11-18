namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Monitoring Events validator.
/// </summary>
public class MonitoringEventsValidator : AbstractValidator<MonitoringEventsDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public MonitoringEventsValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Date)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);

        RuleFor(dto => dto.Value)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);
        });
    }
}
