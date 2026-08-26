namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

using System;

/// <summary>
/// OpenApi request example attribute.
/// </summary>
/// <param name="providerType">Provider type.</param>
[AttributeUsage(
    AttributeTargets.Method,
    AllowMultiple = false,
    Inherited = true)]
public sealed class OpenApiRequestAttribute(Type providerType) : Attribute
{
    /// <summary>
    /// Provider type.
    /// </summary>
    public Type ProviderType { get; } = providerType;
}
