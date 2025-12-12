#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class UpdateInitiativesAndTags : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_initiative_tag_initiative_id",
            schema: "initiatives",
            table: "initiative_tag");

        migrationBuilder.InsertData(
            schema: "initiatives",
            table: "tag_category",
            columns: new[] { "id", "name" },
            values: new object[,]
            {
                { 3, "BiologicalGroup" },
                { 4, "Ecosystem" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_initiative_tag_initiative_id_tag_id",
            schema: "initiatives",
            table: "initiative_tag",
            columns: new[] { "initiative_id", "tag_id" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_initiative_tag_initiative_id_tag_id",
            schema: "initiatives",
            table: "initiative_tag");

        migrationBuilder.DeleteData(
            schema: "initiatives",
            table: "tag_category",
            keyColumn: "id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            schema: "initiatives",
            table: "tag_category",
            keyColumn: "id",
            keyValue: 4);

        migrationBuilder.CreateIndex(
            name: "IX_initiative_tag_initiative_id",
            schema: "initiatives",
            table: "initiative_tag",
            column: "initiative_id");
    }
}
