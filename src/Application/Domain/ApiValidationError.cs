namespace IAVH.BioTablero.CM.Application.Domain;

/// <summary>
/// Api validation error.
/// </summary>
public class ApiValidationError
{
    /// <summary>
    /// Message code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Message description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Validation field.
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Additional data.
    /// </summary>
    public object Data { get; set; }
}
