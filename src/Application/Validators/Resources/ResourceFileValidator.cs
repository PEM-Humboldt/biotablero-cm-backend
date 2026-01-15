namespace IAVH.BioTablero.CM.Application.Validators.Resources;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

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
                .WithMessage("Entity data cannot be null");

        RuleFor(dto => dto.Name)
            .NotEmpty()
                .WithMessage("{PropertyName} is required")
            .MaximumLength(100);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.ResourceId)
                .NotNull()
                    .WithMessage("{PropertyName} is required");
        });
    }
}
