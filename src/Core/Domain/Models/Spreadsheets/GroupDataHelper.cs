namespace IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;

/// <summary>
/// Indicators group (category) data helper.
/// </summary>
public class GroupDataHelper()
{
    /// <summary>
    /// Category identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Category parent identifier.
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// Group name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Group description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Parent name.
    /// </summary>
    public string? ParentName { get; set; }
}
