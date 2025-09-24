namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System.Threading.Tasks;

/// <summary>
/// Web View Tools interface.
/// </summary>
public interface IWebViewTools
{
    /// <summary>
    /// Cast Web view to string.
    /// </summary>
    /// <param name="viewName">View name.</param>
    /// <param name="model">View model data.</param>
    /// <param name="isPartial">Partial view flag.</param>
    /// <returns>Web view as HTML string.</returns>
    Task<string> RenderViewToStringAsync(string viewName, object model, bool isPartial = false);
}
