#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class AddMonitoringEvents : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "monitoring_events",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                value = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_monitoring_events", x => x.id);
                table.ForeignKey(
                    name: "FK_monitoring_events_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_monitoring_events_initiative_id_date",
            schema: "initiatives",
            table: "monitoring_events",
            columns: new[] { "initiative_id", "date" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "monitoring_events",
            schema: "initiatives");
}
