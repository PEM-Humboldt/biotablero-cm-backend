namespace IAVH.BioTablero.CM.WebApi.Config.LoggerSetup.Enrichers;

using IAVH.BioTablero.CM.WebApi.Utils;

using Microsoft.AspNetCore.Http;

using Serilog.Core;
using Serilog.Events;

/// <summary>
/// Serilog custom user name enricher.
/// </summary>
public class UserEnricher(IHttpContextAccessor httpContextAccessor) : ILogEventEnricher
{
    private const string ClientUserPropertyName = "UserName";
    private const string UserAnonymous = "anonymous";
    private const string UserSystem = "SYSTEM";

    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

    /// <inheritdoc/>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var userName = httpContext == null ? UserSystem : httpContext.GetUserName() ?? UserAnonymous;
        var userNameProperty = new LogEventProperty(ClientUserPropertyName, new ScalarValue(userName));

        logEvent?.AddOrUpdateProperty(userNameProperty);
    }
}
