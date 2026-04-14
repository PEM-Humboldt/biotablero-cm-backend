#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class AddNotifications : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "notifications");

        migrationBuilder.CreateTable(
            name: "notification",
            schema: "notifications",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                receiver = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                subject = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                body = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                creation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                reading_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                is_read = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                properties = table.Column<string>(type: "jsonb", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_notification", x => x.id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "notification",
            schema: "notifications");
}
