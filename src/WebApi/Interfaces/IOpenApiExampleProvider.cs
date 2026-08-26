namespace IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// OpenAPI Example Provider interface.
/// </summary>
/// <typeparam name="T">Return class type.</typeparam>
public interface IOpenApiExampleProvider<out T>
{
    /// <summary>
    /// Create examples.
    /// </summary>
    /// <returns>Examples object.</returns>
    T GetExamples();
}
