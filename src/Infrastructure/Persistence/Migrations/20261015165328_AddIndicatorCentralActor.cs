#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class AddIndicatorCentralActor : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "central_actor_id",
            schema: "indicators",
            table: "indicator_value",
            type: "integer",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "central_actor",
            schema: "indicators",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_central_actor", x => x.id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_indicator_value_central_actor_id",
            schema: "indicators",
            table: "indicator_value",
            column: "central_actor_id");

        migrationBuilder.CreateIndex(
            name: "IX_central_actor_name",
            schema: "indicators",
            table: "central_actor",
            column: "name",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_indicator_value_central_actor_central_actor_id",
            schema: "indicators",
            table: "indicator_value",
            column: "central_actor_id",
            principalSchema: "indicators",
            principalTable: "central_actor",
            principalColumn: "id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_indicator_value_central_actor_central_actor_id",
            schema: "indicators",
            table: "indicator_value");

        migrationBuilder.DropTable(
            name: "central_actor",
            schema: "indicators");

        migrationBuilder.DropIndex(
            name: "IX_indicator_value_central_actor_id",
            schema: "indicators",
            table: "indicator_value");

        migrationBuilder.DropColumn(
            name: "central_actor_id",
            schema: "indicators",
            table: "indicator_value");
    }
}
