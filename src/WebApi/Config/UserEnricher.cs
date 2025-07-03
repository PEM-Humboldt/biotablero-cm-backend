namespace IAVH.BioTablero.CM.WebApi.Config;

using IAVH.BioTablero.CM.WebApi.Extensions;

using Microsoft.AspNetCore.Http;

using Serilog.Core;
using Serilog.Events;

/// <summary>
/// Serilog custom user name enricher
/// </summary>
public class UserEnricher(IHttpContextAccessor httpContextAccessor) : ILogEventEnricher
{
    private const string ClientUserPropertyName = "UserName";
    private const string ClientUserItemKey = "Serilog_UserName";
    private const string UserAnonymous = "anonymous";
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

    /// <summary>
    /// Enrich logs with current user name
    /// </summary>
    /// <param name="logEvent">Serilog log event</param>
    /// <param name="propertyFactory">Serilog property factory</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            return;
        }

        if (httpContext.Items[ClientUserItemKey] is LogEventProperty logEventProperty)
        {
            logEvent.AddPropertyIfAbsent(logEventProperty);
            return;
        }

        string userName = httpContext?.GetUserName() ?? UserAnonymous;

        var userNameProperty = new LogEventProperty(ClientUserPropertyName, new ScalarValue(userName));
        httpContext.Items.Add(ClientUserItemKey, userNameProperty);

        logEvent.AddPropertyIfAbsent(userNameProperty);
    }
}
