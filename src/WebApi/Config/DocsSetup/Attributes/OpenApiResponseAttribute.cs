namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

using System;

/// <summary>
/// OpenApi response example attribute.
/// </summary>
/// <param name="statusCode">HTTP status code.</param>
/// <param name="providerType">Provider type.</param>
[AttributeUsage(
    AttributeTargets.Method,
    AllowMultiple = false,
    Inherited = true)]
public sealed class OpenApiResponseAttribute(int statusCode, Type providerType) : Attribute
{
    /// <summary>
    /// HTTP status code.
    /// </summary>
    public int StatusCode { get; } = statusCode;

    /// <summary>
    /// Provider type.
    /// </summary>
    public Type ProviderType { get; } = providerType;
}
