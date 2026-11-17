#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class AddInitiativeJoinInvitations : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "join_invitation",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                creator_user_name = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                message = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: true),
                creation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_join_invitation", x => x.id);
                table.ForeignKey(
                    name: "FK_join_invitation_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "join_invitation_guest",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                join_invitation_id = table.Column<int>(type: "integer", nullable: false),
                email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_join_invitation_guest", x => x.id);
                table.ForeignKey(
                    name: "FK_join_invitation_guest_join_invitation_join_invitation_id",
                    column: x => x.join_invitation_id,
                    principalSchema: "initiatives",
                    principalTable: "join_invitation",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_join_invitation_initiative_id",
            schema: "initiatives",
            table: "join_invitation",
            column: "initiative_id");

        migrationBuilder.CreateIndex(
            name: "IX_join_invitation_guest_join_invitation_id_email",
            schema: "initiatives",
            table: "join_invitation_guest",
            columns: new[] { "join_invitation_id", "email" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "join_invitation_guest",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "join_invitation",
            schema: "initiatives");
    }
}
