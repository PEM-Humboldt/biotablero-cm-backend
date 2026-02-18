namespace IAVH.BioTablero.CM.Application.Validators.Resources;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

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
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithMessage("{PropertyName} is required")
            .MaximumLength(100);

        RuleFor(dto => dto.Description)
            .NotEmpty()
                .WithMessage("{PropertyName} is required")
            .MaximumLength(500);

        RuleFor(dto => dto.ResourceType)
            .NotNull()
                .WithMessage("{PropertyName} is required");

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.InitiativeId)
                .NotNull()
                    .WithMessage("{PropertyName} is required");

            RuleForEach(dto => dto.Files)
                .SetValidator(new ResourceFileValidator(), "default");

            RuleForEach(dto => dto.Links)
                .SetValidator(new ResourceLinkValidator(), "default");
        });
    }
}
