#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class UpdateInitiativeAttributeBaseline : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.RenameColumn(
            name: "influence_area",
            schema: "initiatives",
            table: "initiative",
            newName: "baseline");

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.RenameColumn(
            name: "baseline",
            schema: "initiatives",
            table: "initiative",
            newName: "influence_area");
}
