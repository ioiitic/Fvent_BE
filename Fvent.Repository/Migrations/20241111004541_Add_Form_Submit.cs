using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fvent.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Add_Form_Submit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId1",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_EventFiles_EventId",
                table: "EventFiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConversationId1",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "FileType",
                table: "EventFiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "FormSubmits",
                columns: table => new
                {
                    FormSubmitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormSubmits", x => x.FormSubmitId);
                    table.ForeignKey(
                        name: "FK_FormSubmits_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormSubmits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventFiles_EventId",
                table: "EventFiles",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmits_EventId",
                table: "FormSubmits",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmits_UserId",
                table: "FormSubmits",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId1",
                table: "Messages",
                column: "ConversationId1",
                principalTable: "Conversations",
                principalColumn: "ConversationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId1",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "FormSubmits");

            migrationBuilder.DropIndex(
                name: "IX_EventFiles_EventId",
                table: "EventFiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConversationId1",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FileType",
                table: "EventFiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventFiles_EventId",
                table: "EventFiles",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId1",
                table: "Messages",
                column: "ConversationId1",
                principalTable: "Conversations",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
