#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class UpdateJoinRequestsLeave : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_join_request_initiative_user_level_initiative_user_level_id",
            schema: "initiatives",
            table: "join_request");

        migrationBuilder.AlterColumn<int>(
            name: "initiative_user_level_id",
            schema: "initiatives",
            table: "join_request",
            type: "integer",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AddForeignKey(
            name: "FK_join_request_initiative_user_level_initiative_user_level_id",
            schema: "initiatives",
            table: "join_request",
            column: "initiative_user_level_id",
            principalSchema: "initiatives",
            principalTable: "initiative_user_level",
            principalColumn: "id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_join_request_initiative_user_level_initiative_user_level_id",
            schema: "initiatives",
            table: "join_request");

        migrationBuilder.AlterColumn<int>(
            name: "initiative_user_level_id",
            schema: "initiatives",
            table: "join_request",
            type: "integer",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_join_request_initiative_user_level_initiative_user_level_id",
            schema: "initiatives",
            table: "join_request",
            column: "initiative_user_level_id",
            principalSchema: "initiatives",
            principalTable: "initiative_user_level",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }
}
