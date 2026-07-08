#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class UpdateInitiativesAndTags2 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "tags");

        migrationBuilder.RenameTable(
            name: "tag_category",
            schema: "initiatives",
            newName: "tag_category",
            newSchema: "tags");

        migrationBuilder.RenameTable(
            name: "tag",
            schema: "initiatives",
            newName: "tag",
            newSchema: "tags");

        migrationBuilder.AlterColumn<string>(
            name: "description",
            schema: "initiatives",
            table: "initiative",
            type: "character varying(1000)",
            maxLength: 1000,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(300)",
            oldMaxLength: 300);

        migrationBuilder.AddColumn<string>(
            name: "full_name",
            schema: "tags",
            table: "tag",
            type: "character varying(120)",
            maxLength: 120,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "full_name",
            schema: "tags",
            table: "tag");

        migrationBuilder.RenameTable(
            name: "tag_category",
            schema: "tags",
            newName: "tag_category",
            newSchema: "initiatives");

        migrationBuilder.RenameTable(
            name: "tag",
            schema: "tags",
            newName: "tag",
            newSchema: "initiatives");

        migrationBuilder.AlterColumn<string>(
            name: "description",
            schema: "initiatives",
            table: "initiative",
            type: "character varying(300)",
            maxLength: 300,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(1000)",
            oldMaxLength: 1000);
    }
}
