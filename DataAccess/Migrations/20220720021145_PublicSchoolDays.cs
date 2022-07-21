using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class PublicSchoolDays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Friday",
                table: "PublicSchoolSchedules");

            migrationBuilder.DropColumn(
                name: "Monday",
                table: "PublicSchoolSchedules");

            migrationBuilder.DropColumn(
                name: "Thursday",
                table: "PublicSchoolSchedules");

            migrationBuilder.DropColumn(
                name: "Tuesday",
                table: "PublicSchoolSchedules");

            migrationBuilder.DropColumn(
                name: "Wednesday",
                table: "PublicSchoolSchedules");

            migrationBuilder.AddColumn<int>(
                name: "weekday",
                table: "PublicSchoolSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "weekday",
                table: "PublicSchoolSchedules");

            migrationBuilder.AddColumn<bool>(
                name: "Friday",
                table: "PublicSchoolSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Monday",
                table: "PublicSchoolSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Thursday",
                table: "PublicSchoolSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Tuesday",
                table: "PublicSchoolSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Wednesday",
                table: "PublicSchoolSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
