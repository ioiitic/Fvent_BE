using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fvent.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckIn",
                table: "EventRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    RefreshTokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.RefreshTokenId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RefreshTokenId",
                table: "Users",
                column: "RefreshTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_RefreshToken_RefreshTokenId",
                table: "Users",
                column: "RefreshTokenId",
                principalTable: "RefreshToken",
                principalColumn: "RefreshTokenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_RefreshToken_RefreshTokenId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_Users_RefreshTokenId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsCheckIn",
                table: "EventRegistrations");
        }
    }
}
