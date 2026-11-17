namespace IAVH.BioTablero.CM.Application.DTOs.Reports;

/// <summary>
/// Report column configuration.
/// </summary>
public class ReportColumnConfig
{
    /// <summary>
    /// Column header.
    /// </summary>
    public string Header { get; set; } = string.Empty;

    /// <summary>
    /// Column index.
    /// </summary>
    public int Index { get; set; } = int.MaxValue;

    /// <summary>
    /// Visible column flag.
    /// </summary>
    public bool Visible { get; set; }
}
