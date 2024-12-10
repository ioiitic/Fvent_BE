using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fvent.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRegistrationAndEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubMaxAttendees",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckinTime",
                table: "EventRegistrations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReminderSent30",
                table: "EventRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReminderSent60",
                table: "EventRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubMaxAttendees",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CheckinTime",
                table: "EventRegistrations");

            migrationBuilder.DropColumn(
                name: "IsReminderSent30",
                table: "EventRegistrations");

            migrationBuilder.DropColumn(
                name: "IsReminderSent60",
                table: "EventRegistrations");
        }
    }
}
