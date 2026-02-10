namespace IAVH.BioTablero.CM.Application.Validators.Resources;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Resource validator.
/// </summary>
public class ResourceValidator : AbstractValidator<ResourceDto>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public ResourceValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyEntityData);

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(100);

        RuleFor(dto => dto.Description)
            .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty)
            .MaximumLength(500);

        RuleFor(dto => dto.ResourceType)
            .NotNull()
                .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithErrorCode(ValidationErrorCodes.General.EmptyProperty);

            RuleForEach(dto => dto.Files)
                .SetValidator(new ResourceFileValidator(), "default");

            RuleForEach(dto => dto.Links)
                .SetValidator(new ResourceLinkValidator(), "default");
        });
    }
}
