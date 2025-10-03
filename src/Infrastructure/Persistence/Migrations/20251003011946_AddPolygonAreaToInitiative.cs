#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class AddPolygonAreaToInitiative : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) =>
        migrationBuilder.AddColumn<decimal>(
            name: "polygon_area",
            schema: "initiatives",
            table: "initiative",
            type: "decimal(15,6)",
            nullable: false,
            defaultValue: 0m);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.DropColumn(
            name: "polygon_area",
            schema: "initiatives",
            table: "initiative");
}
