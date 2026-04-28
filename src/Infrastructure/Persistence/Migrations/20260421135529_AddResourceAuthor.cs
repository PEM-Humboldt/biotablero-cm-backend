#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class AddResourceAuthor : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<string>(
            name: "author_user_name",
            schema: "initiatives",
            table: "resource",
            type: "character varying(75)",
            maxLength: 75,
            nullable: false,
            defaultValue: string.Empty);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
            name: "author_user_name",
            schema: "initiatives",
            table: "resource");
}
