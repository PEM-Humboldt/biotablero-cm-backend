#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class UpdateTerritoryStoryVideoConstraints : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_territory_story_video_file_url",
            schema: "initiatives",
            table: "territory_story_video");

        migrationBuilder.DropIndex(
            name: "IX_territory_story_video_territory_story_id",
            schema: "initiatives",
            table: "territory_story_video");

        migrationBuilder.CreateIndex(
            name: "IX_territory_story_video_territory_story_id_file_url",
            schema: "initiatives",
            table: "territory_story_video",
            columns: new[] { "territory_story_id", "file_url" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_territory_story_video_territory_story_id_file_url",
            schema: "initiatives",
            table: "territory_story_video");

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
}
