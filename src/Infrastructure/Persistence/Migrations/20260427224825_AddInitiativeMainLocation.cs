#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class AddInitiativeMainLocation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "main_location_id",
            schema: "initiatives",
            table: "initiative",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_initiative_main_location_id",
            schema: "initiatives",
            table: "initiative",
            column: "main_location_id");

        migrationBuilder.AddForeignKey(
            name: "FK_initiative_location_main_location_id",
            schema: "initiatives",
            table: "initiative",
            column: "main_location_id",
            principalSchema: "geo",
            principalTable: "location",
            principalColumn: "id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_initiative_location_main_location_id",
            schema: "initiatives",
            table: "initiative");

        migrationBuilder.DropIndex(
            name: "IX_initiative_main_location_id",
            schema: "initiatives",
            table: "initiative");

        migrationBuilder.DropColumn(
            name: "main_location_id",
            schema: "initiatives",
            table: "initiative");
    }
}
