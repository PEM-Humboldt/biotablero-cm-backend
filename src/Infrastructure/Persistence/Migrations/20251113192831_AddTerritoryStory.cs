#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

/// <inheritdoc />
public partial class AddTerritoryStory : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "territory_story",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                initiative_id = table.Column<int>(type: "integer", nullable: false),
                author_user_name = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                keywords = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: true),
                creation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                restricted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                featured_content = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_territory_story", x => x.id);
                table.ForeignKey(
                    name: "FK_territory_story_initiative_initiative_id",
                    column: x => x.initiative_id,
                    principalSchema: "initiatives",
                    principalTable: "initiative",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "territory_story_image",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                territory_story_id = table.Column<int>(type: "integer", nullable: false),
                file_url = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                featured_content = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_territory_story_image", x => x.id);
                table.ForeignKey(
                    name: "FK_territory_story_image_territory_story_territory_story_id",
                    column: x => x.territory_story_id,
                    principalSchema: "initiatives",
                    principalTable: "territory_story",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "territory_story_like",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                territory_story_id = table.Column<int>(type: "integer", nullable: false),
                user_name = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                creation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_territory_story_like", x => x.id);
                table.ForeignKey(
                    name: "FK_territory_story_like_territory_story_territory_story_id",
                    column: x => x.territory_story_id,
                    principalSchema: "initiatives",
                    principalTable: "territory_story",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "territory_story_video",
            schema: "initiatives",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                territory_story_id = table.Column<int>(type: "integer", nullable: false),
                file_url = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_territory_story_video", x => x.id);
                table.ForeignKey(
                    name: "FK_territory_story_video_territory_story_territory_story_id",
                    column: x => x.territory_story_id,
                    principalSchema: "initiatives",
                    principalTable: "territory_story",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_territory_story_initiative_id",
            schema: "initiatives",
            table: "territory_story",
            column: "initiative_id");

        migrationBuilder.CreateIndex(
            name: "IX_territory_story_title",
            schema: "initiatives",
            table: "territory_story",
            column: "title",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_territory_story_image_file_url",
            schema: "initiatives",
            table: "territory_story_image",
            column: "file_url",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_territory_story_image_territory_story_id",
            schema: "initiatives",
            table: "territory_story_image",
            column: "territory_story_id");

        migrationBuilder.CreateIndex(
            name: "IX_territory_story_like_territory_story_id_user_name",
            schema: "initiatives",
            table: "territory_story_like",
            columns: new[] { "territory_story_id", "user_name" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_territory_story_video_file_url",
            schema: "initiatives",
            table: "territory_story_video",
            column: "file_url",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_territory_story_video_territory_story_id",
            schema: "initiatives",
            table: "territory_story_video",
            column: "territory_story_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "territory_story_image",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "territory_story_like",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "territory_story_video",
            schema: "initiatives");

        migrationBuilder.DropTable(
            name: "territory_story",
            schema: "initiatives");
    }
}
