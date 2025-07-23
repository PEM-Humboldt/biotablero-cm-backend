namespace IAVH.BioTablero.CM.WebApi;

using System.Text.Json.Serialization;

using DotNetEnv;

using IAVH.BioTablero.CM.Persistence.Config.DependencyRegistry;
using IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup;
using IAVH.BioTablero.CM.WebApi.Config.LoggerSetup;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Main program class
/// </summary>
public class Program
{
    /// <summary>
    /// Main function
    /// </summary>
    /// <param name="args">System arguments</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Load environment variables from a local file
        Env.Load("../../.env");

        // Add DB Contexts
        ConfigDbDependencies.AddDbServices(builder.Services);

        // Dependency injection configuration
        builder.Services.AddCoreServices();
        builder.Services.AddAppServices();
        builder.Services.AddMappings();

        // Add services to the container.
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Serialize enums as strings in API responses
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddOData(options =>
            {
                // Add OData default settings
                options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(50);
            });

        // Logs setup
        builder.Host.AddLogConfig(builder.Services);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddCustomOptions();
        });

        // Enable Swagger custom examples
        builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseDeveloperExceptionPage();

            // Global CORS policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        }

        // Setup health checks endpoint
        app.MapHealthChecks("/health");

        // Add support to logging request with Serilog
        app.UseSerilogRequestLogging();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
