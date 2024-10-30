using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fvent.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Update_Event_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventStatuses_StatusId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "EventStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Events_StatusId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Events",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Events",
                newName: "StatusId");

            migrationBuilder.CreateTable(
                name: "EventStatuses",
                columns: table => new
                {
                    EventStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStatuses", x => x.EventStatusId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_StatusId",
                table: "Events",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventStatuses_StatusId",
                table: "Events",
                column: "StatusId",
                principalTable: "EventStatuses",
                principalColumn: "EventStatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
