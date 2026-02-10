namespace IAVH.BioTablero.CM.Application.Validators.Initiatives;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Initiative location validator.
/// </summary>
public class InitiativeLocationValidator : AbstractValidator<InitiativeLocationDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeLocationValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.LocationId)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);

        RuleFor(dto => dto.Locality)
            .MaximumLength(300);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);
        });
    }
}
