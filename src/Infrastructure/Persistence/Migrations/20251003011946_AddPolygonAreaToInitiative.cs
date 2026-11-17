#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class AddPolygonAreaToInitiative : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) =>
        migrationBuilder.AddColumn<double>(
            name: "polygon_area",
            schema: "initiatives",
            table: "initiative",
            type: "double precision",
            nullable: false,
            defaultValue: 0.0);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.DropColumn(
            name: "polygon_area",
            schema: "initiatives",
            table: "initiative");
}
