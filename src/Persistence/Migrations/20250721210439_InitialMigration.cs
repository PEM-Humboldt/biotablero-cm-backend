#nullable disable

namespace IAVH.BioTablero.CM.Persistence.Migrations;

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
                type = table.Column<int>(type: "integer", nullable: true),
                message = table.Column<string>(type: "text", nullable: true),
                user_name = table.Column<string>(type: "text", nullable: true),
                custom_record = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                client_ip = table.Column<string>(type: "text", nullable: true),
                client_agent = table.Column<string>(type: "text", nullable: true),
                properties = table.Column<string>(type: "jsonb", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_logs", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "Logs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Level = table.Column<int>(type: "integer", nullable: false),
                Type = table.Column<int>(type: "integer", nullable: true),
                Message = table.Column<string>(type: "text", nullable: true),
                UserName = table.Column<string>(type: "text", nullable: true),
                CustomRecord = table.Column<bool>(type: "boolean", nullable: false),
                ClientIp = table.Column<string>(type: "text", nullable: true),
                ClientAgent = table.Column<string>(type: "text", nullable: true),
                Properties = table.Column<string>(type: "text", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Logs", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "logs",
            schema: "logs");

        migrationBuilder.DropTable(
            name: "Logs");
    }
}
