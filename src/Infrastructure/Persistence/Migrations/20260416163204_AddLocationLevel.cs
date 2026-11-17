#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class AddLocationLevel : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<byte>(
            name: "level",
            schema: "geo",
            table: "location",
            type: "smallint",
            nullable: false,
            defaultValue: (byte)0);

        migrationBuilder.AddCheckConstraint(
            name: "CK_Location_Level_ValidValues",
            schema: "geo",
            table: "location",
            sql: "\"level\" IN (1, 2, 3)");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropCheckConstraint(
            name: "CK_Location_Level_ValidValues",
            schema: "geo",
            table: "location");

        migrationBuilder.DropColumn(
            name: "level",
            schema: "geo",
            table: "location");
    }
}
