#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class AddInitiativeJoinRequests : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "initiative_join_request_status",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_initiative_join_request_status", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "initiative_join_request",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                user_name = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                reviewer_user_name = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: true),
                creation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                response_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                status_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_initiative_join_request", x => x.id);
                table.ForeignKey(
                    name: "FK_initiative_join_request_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_initiative_join_request_initiative_join_request_status_stat~",
                    column: x => x.status_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative_join_request_status",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            schema: "initiatives",
            table: "initiative_join_request_status",
            columns: new[] { "id", "name" },
            values: new object[,]
            {
                { 1, "UnderReview" },
                { 2, "Approved" },
                { 3, "Rejected" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_initiative_join_request_initiative_id",
            schema: "initiatives",
            table: "initiative_join_request",
            column: "initiative_id");

        migrationBuilder.CreateIndex(
            name: "IX_initiative_join_request_status_id",
            schema: "initiatives",
            table: "initiative_join_request",
            column: "status_id");

        migrationBuilder.CreateIndex(
            name: "IX_initiative_join_request_status_name",
            schema: "initiatives",
            table: "initiative_join_request_status",
            column: "name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "initiative_join_request",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "initiative_join_request_status",
            schema: "initiatives");
    }
}
