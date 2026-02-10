namespace IAVH.BioTablero.CM.Application.Validators.Resources;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

/// <summary>
/// Resource link validator.
/// </summary>
public class ResourceLinkValidator : AbstractValidator<ResourceLinkDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public ResourceLinkValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyEntityData);

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
            .MaximumLength(100);

        RuleFor(dto => dto.Url)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty)
            .Matches(RegExprConstants.Url)
            .MaximumLength(250);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.ResourceId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.GeneralEmptyProperty);
        });
    }
}
