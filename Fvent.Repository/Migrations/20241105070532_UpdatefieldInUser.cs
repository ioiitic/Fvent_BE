using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fvent.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdatefieldInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "ProcessNote");

            migrationBuilder.AlterColumn<int>(
                name: "Verified",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProcessNote",
                table: "Users",
                newName: "LastName");

            migrationBuilder.AlterColumn<bool>(
                name: "Verified",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
