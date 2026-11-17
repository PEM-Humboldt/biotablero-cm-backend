#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class FixTagUniqueRules : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_tag_name",
            schema: "initiatives",
            table: "tag");

        migrationBuilder.CreateIndex(
            name: "IX_tag_name_tag_category_id",
            schema: "initiatives",
            table: "tag",
            columns: new[] { "name", "tag_category_id" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_tag_name_tag_category_id",
            schema: "initiatives",
            table: "tag");

        migrationBuilder.CreateIndex(
            name: "IX_tag_name",
            schema: "initiatives",
            table: "tag",
            column: "name",
            unique: true);
    }
}
