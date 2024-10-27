using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fvent.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddSvgContentToTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SvgContent",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SvgContent",
                table: "Tags");
        }
    }
}
