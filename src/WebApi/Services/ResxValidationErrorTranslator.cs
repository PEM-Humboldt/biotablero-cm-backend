namespace IAVH.BioTablero.CM.WebApi.Services;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using FluentValidation.Results;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.WebApi.Resources;

using Microsoft.Extensions.Localization;

/// <summary>
/// Validator error translator for resx files.
/// </summary>
public class ResxValidationErrorTranslator
    : IValidationErrorTranslator
{
    private readonly IStringLocalizer<ValidationMessages> localizer;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="localizer">String localizer.</param>
    public ResxValidationErrorTranslator(
        IStringLocalizer<ValidationMessages> localizer)
    {
        this.localizer = localizer;
    }

    /// <inheritdoc/>
    public ApiValidationError Translate(string errorCode, string propertyName = null, object data = null) =>
        new()
        {
            Code = errorCode,
            Description = localizer[errorCode],
            Field = propertyName,
            Data = data,
        };

    /// <inheritdoc/>
    public IEnumerable<ApiValidationError> Translate(
        IEnumerable<ValidationFailure> failures)
    {
        foreach (var f in failures)
        {
            var template = localizer[f.ErrorCode].Value;

            template = template.Replace(
                "{PropertyName}",
                f.PropertyName);

            var message = f.CustomState is null
                ? template
                : string.Format(
                    CultureInfo.CurrentCulture,
                    template,
                    [.. f.CustomState.GetType()
                        .GetProperties()
                        .Select(p => p.GetValue(f.CustomState))]);

            yield return new ApiValidationError
            {
                Code = f.ErrorCode,
                Description = message,
                Field = f.PropertyName,
            };
        }
    }
}
