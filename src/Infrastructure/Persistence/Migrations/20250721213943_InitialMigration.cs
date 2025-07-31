namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

#nullable disable

using System;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "logs");

        migrationBuilder.CreateTable(
            name: "logs",
            schema: "logs",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                level = table.Column<int>(type: "integer", nullable: false),
                type = table.Column<int>(type: "integer", nullable: false),
                message = table.Column<string>(type: "text", nullable: true),
                user_name = table.Column<string>(type: "text", nullable: true),
                custom_record = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                client_ip = table.Column<string>(type: "text", nullable: true),
                client_agent = table.Column<string>(type: "text", nullable: true),
                properties = table.Column<string>(type: "jsonb", nullable: false),
            });

        // Create index on identifier
        migrationBuilder.Sql("CREATE INDEX ON logs.logs (id);");

        // Convert table to hypertable (TimescaleDB)
        migrationBuilder.Sql("SELECT create_hypertable('logs.logs', 'timestamp');");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "logs",
            schema: "logs");
}
