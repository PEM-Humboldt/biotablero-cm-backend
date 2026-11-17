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
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(100)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleFor(dto => dto.Url)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .Matches(RegExprConstants.Url)
                .WithErrorCode(ValidationErrorCodes.General.InvalidPropertyValue)
            .MaximumLength(250)
                .WithErrorCode(ValidationErrorCodes.General.InvalidTextLength);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.ResourceId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);
        });
    }
}
