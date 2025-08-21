#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class AddInitiativeTags : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "initiative_tag_category",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_initiative_tag_category", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "initiative_tag",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                url = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                category_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_initiative_tag", x => x.id);
                table.ForeignKey(
                    name: "FK_initiative_tag_initiative_tag_category_category_id",
                    column: x => x.category_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative_tag_category",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "initiative_tag_initiative",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                tag_id = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_initiative_tag_initiative", x => x.id);
                table.ForeignKey(
                    name: "FK_initiative_tag_initiative_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_initiative_tag_initiative_initiative_tag_tag_id",
                    column: x => x.tag_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative_tag",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            schema: "initiatives",
            table: "initiative_tag_category",
            columns: new[] { "id", "name" },
            values: new object[,]
            {
                { 1, "PoliticalContext" },
                { 2, "SocialContext" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_initiative_tag_category_id",
            schema: "initiatives",
            table: "initiative_tag",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "IX_initiative_tag_name",
            schema: "initiatives",
            table: "initiative_tag",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_initiative_tag_category_name",
            schema: "initiatives",
            table: "initiative_tag_category",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_initiative_tag_initiative_initiative_id",
            schema: "initiatives",
            table: "initiative_tag_initiative",
            column: "initiative_id");

        migrationBuilder.CreateIndex(
            name: "IX_initiative_tag_initiative_tag_id",
            schema: "initiatives",
            table: "initiative_tag_initiative",
            column: "tag_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "initiative_tag_initiative",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "initiative_tag",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "initiative_tag_category",
            schema: "initiatives");
    }
}
