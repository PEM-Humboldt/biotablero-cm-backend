namespace IAVH.BioTablero.CM.WebApi.Config.LoggerSetup;

using System;
using System.Collections.Generic;
using System.Globalization;

using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.WebApi.Config.LoggerSetup.ColumWriters;
using IAVH.BioTablero.CM.WebApi.Config.LoggerSetup.Enrichers;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Serilog;
using Serilog.Enrichers;
using Serilog.Sinks.PostgreSQL;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Logger configuration
/// </summary>
public static class ConfigLogProperties
{
    /// <summary>
    /// System log configuration
    /// </summary>
    /// <param name="host">Host builder</param>
    /// <param name="services">Application services</param>
    /// <returns>Host builder configuration</returns>
    public static ConfigureHostBuilder AddLogConfig(this ConfigureHostBuilder host, IServiceCollection services)
    {
        var columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            { "id", new GuidColumnWriter("Id") },
            { "timestamp", new TimestampColumnWriter() },
            { "level", new LevelColumnWriter() },
            { "type", new IntegerColumnWriter("Type") },
            { "message", new RenderedMessageColumnWriter(NpgsqlTypes.NpgsqlDbType.Text) },
            { "user_name", new RawStringColumnWriter("UserName") },
            { "custom_record", new BoolColumnWriter("CustomRecord") },
            { "client_ip", new RawStringColumnWriter("ClientIp") },
            { "client_agent", new RawStringColumnWriter("ClientAgent") },
            { "properties", new LogEventSerializedColumnWriter(NpgsqlTypes.NpgsqlDbType.Jsonb) },
        };

        // General setup
        host.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration
                    .Enrich.With<IdEnricher>()
                    .Enrich.WithProperty(LogConstants.ApplicationName, LogConstants.ProjectName)
                    .Enrich.WithProperty(LogConstants.CustomRecord, false)
                    .Enrich.WithProperty(LogConstants.CustomType, (int)LogType.System)
                    .Enrich.With(new UserEnricher(services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>()))
                    .Enrich.With(new ClientIpEnricher())
                    .Enrich.With(new ClientHeaderEnricher("User-Agent", "ClientAgent"))
                    .ReadFrom.Configuration(context.Configuration)
                    .WriteTo.Logger(lc => lc

                        // Discard SQL Command logs
                        .Filter.ByExcluding(
                            e => e.Properties.ContainsKey("SourceContext") && e.Properties["SourceContext"].ToString()
                                .Contains("Microsoft.EntityFrameworkCore.Database.Command", StringComparison.CurrentCultureIgnoreCase))

                        // Discard HTTP Requests logs
                        .Filter.ByExcluding(
                            e => e.Properties.ContainsKey("SourceContext") && e.Properties["SourceContext"].ToString()
                                .Contains("Serilog.AspNetCore.RequestLoggingMiddleware", StringComparison.CurrentCultureIgnoreCase))

                        .WriteTo.PostgreSQL(
                            connectionString: Environment.GetEnvironmentVariable("CS_MAIN"),
                            schemaName: LogConstants.DefaultSchemaName,
                            tableName: LogConstants.DefaultTableName,
                            needAutoCreateTable: false,
                            columnOptions: columnWriters,
                            formatProvider: new CultureInfo("es-CO")));
            });

        return host;
    }

    /// <summary>
    /// Serilog configuration for ASP.NET module
    /// </summary>
    /// <param name="diagnosticContext">Serilog diagnostic information</param>
    /// <param name="httpContext">HTTP Context</param>
    public static void PushProperties(IDiagnosticContext diagnosticContext, HttpContext httpContext) =>

        // Add HTTP host
        diagnosticContext?.Set("HttpHost", $"{httpContext?.Request.Host}{httpContext.Request.PathBase}");
}
