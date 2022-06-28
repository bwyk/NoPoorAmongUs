using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class Splitnametofirstandlast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Instructors",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Guardians",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Guardians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Guardians");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Instructors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Guardians",
                newName: "Name");
        }
    }
}
