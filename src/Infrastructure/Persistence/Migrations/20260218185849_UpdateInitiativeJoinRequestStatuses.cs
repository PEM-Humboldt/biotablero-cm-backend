#nullable disable

namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class UpdateInitiativeJoinRequestStatuses : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.InsertData(
            schema: "initiatives",
            table: "join_request_status",
            columns: new[] { "id", "name" },
            values: new object[] { 4, "Cancelled" });

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DeleteData(
            schema: "initiatives",
            table: "join_request_status",
            keyColumn: "id",
            keyValue: 4);
}
