using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class RatingsCom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Determination",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "FoodAssistance",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "SchoolLevel",
                table: "Ratings",
                newName: "Distance");      


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicSchoolSchedules");

            migrationBuilder.RenameColumn(
                name: "Distance",
                table: "Ratings",
                newName: "SchoolLevel");

            migrationBuilder.AddColumn<int>(
                name: "Determination",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FoodAssistance",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
