namespace IAVH.BioTablero.CM.WebApi.Config;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Constants;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Serilog;
using Serilog.Sinks.PostgreSQL;

/// <summary>
/// Logger configuration
/// </summary>
public static class ConfigLogProperties
{
    /// <summary>
    /// System log configuration
    /// </summary>
    /// <param name="host">Host builder</param>
    /// <returns>Host builder configuration</returns>
    public static ConfigureHostBuilder AddLogConfig(this ConfigureHostBuilder host)
    {
        var columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            { "id", new SinglePropertyColumnWriter("Id", dbType: NpgsqlTypes.NpgsqlDbType.Varchar) },
            { "timestamp", new TimestampColumnWriter() },
            { "level", new LevelColumnWriter(true, NpgsqlTypes.NpgsqlDbType.Varchar) },
            { "message", new RenderedMessageColumnWriter(NpgsqlTypes.NpgsqlDbType.Text) },
            { "exception", new ExceptionColumnWriter(NpgsqlTypes.NpgsqlDbType.Text) },
            { "properties", new LogEventSerializedColumnWriter(NpgsqlTypes.NpgsqlDbType.Jsonb) },
            { "environment", new SinglePropertyColumnWriter("Environment", dbType: NpgsqlTypes.NpgsqlDbType.Text) },
        };

        // General setup
        host.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration
                    .Enrich.With<UniqueIdEnricher>()
                    .Enrich.WithProperty(LogConstants.ApplicationName, LogConstants.ProjectName)
                    .Enrich.WithProperty(LogConstants.CustomRecord, false)
                    .ReadFrom.Configuration(context.Configuration)
                    .WriteTo.Logger(lc => lc
                        .Filter.ByExcluding(
                            e => e.Properties.ContainsKey("SourceContext") && e.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore.Database.Command"))
                        .WriteTo.PostgreSQL(
                            connectionString: Environment.GetEnvironmentVariable("CS_MAIN"),
                            schemaName: LogConstants.DefaultSchemaName,
                            tableName: LogConstants.DefaultTableName,
                            needAutoCreateTable: true,
                            columnOptions: columnWriters));
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
        diagnosticContext.Set("HttpHost", $"{httpContext.Request.Host}{httpContext.Request.PathBase}");
}
