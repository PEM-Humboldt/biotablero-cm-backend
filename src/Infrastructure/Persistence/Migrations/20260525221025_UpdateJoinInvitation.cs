#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class UpdateJoinInvitation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<string>(
            name: "html_message",
            schema: "initiatives",
            table: "join_invitation",
            type: "text",
            nullable: false,
            defaultValue: string.Empty);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
            name: "html_message",
            schema: "initiatives",
            table: "join_invitation");
}
