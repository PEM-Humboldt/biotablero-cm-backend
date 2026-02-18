namespace IAVH.BioTablero.CM.Application.Validators.Resources;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
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
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithMessage("{PropertyName} is required")
            .MaximumLength(100);

        RuleFor(dto => dto.Url)
            .NotEmpty()
                .WithMessage("{PropertyName} is required")
            .Matches(RegExprConstants.Url)
            .MaximumLength(250);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.ResourceId)
                .NotNull()
                    .WithMessage("{PropertyName} is required");
        });
    }
}
