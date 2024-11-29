using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fvent.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixFormSubmit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormSubmits_UserId",
                table: "FormSubmits");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmits_UserId",
                table: "FormSubmits",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormSubmits_UserId",
                table: "FormSubmits");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmits_UserId",
                table: "FormSubmits",
                column: "UserId",
                unique: true);
        }
    }
}
