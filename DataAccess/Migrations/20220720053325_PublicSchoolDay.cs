using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class PublicSchoolDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "weekday",
                table: "PublicSchoolSchedules",
                newName: "Weekday");

            migrationBuilder.AlterColumn<string>(
                name: "Weekday",
                table: "PublicSchoolSchedules",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weekday",
                table: "PublicSchoolSchedules",
                newName: "weekday");

            migrationBuilder.AlterColumn<int>(
                name: "weekday",
                table: "PublicSchoolSchedules",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
