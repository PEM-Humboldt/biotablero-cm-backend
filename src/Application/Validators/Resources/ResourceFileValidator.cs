namespace IAVH.BioTablero.CM.Application.Validators.Resources;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Resource file validator.
/// </summary>
public class ResourceFileValidator : AbstractValidator<ResourceFileDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public ResourceFileValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyEntityData);

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
            .MaximumLength(100);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.ResourceId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty);
        });
    }
}
