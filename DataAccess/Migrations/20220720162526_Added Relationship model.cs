using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class AddedRelationshipmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guardians_Students_StudentId",
                table: "Guardians");

            migrationBuilder.DropIndex(
                name: "IX_Guardians_StudentId",
                table: "Guardians");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Determination",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "FoodAssistance",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Relationship",
                table: "Guardians");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Guardians");

            migrationBuilder.RenameColumn(
                name: "SchoolLevel",
                table: "Ratings",
                newName: "Distance");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Guardians",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Guardians",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "PublicSchoolSchedules",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SchoolId = table.Column<int>(type: "int", nullable: false),
            //        Monday = table.Column<bool>(type: "bit", nullable: false),
            //        Tuesday = table.Column<bool>(type: "bit", nullable: false),
            //        Wednesday = table.Column<bool>(type: "bit", nullable: false),
            //        Thursday = table.Column<bool>(type: "bit", nullable: false),
            //        Friday = table.Column<bool>(type: "bit", nullable: false),
            //        StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        StudentId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PublicSchoolSchedules", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_PublicSchoolSchedules_Schools_SchoolId",
            //            column: x => x.SchoolId,
            //            principalTable: "Schools",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_PublicSchoolSchedules_Students_StudentId",
            //            column: x => x.StudentId,
            //            principalTable: "Students",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationshipType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    GuardianId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relationships_Guardians_GuardianId",
                        column: x => x.GuardianId,
                        principalTable: "Guardians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relationships_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_PublicSchoolSchedules_SchoolId",
            //    table: "PublicSchoolSchedules",
            //    column: "SchoolId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PublicSchoolSchedules_StudentId",
            //    table: "PublicSchoolSchedules",
            //    column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_GuardianId",
                table: "Relationships",
                column: "GuardianId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_StudentId",
                table: "Relationships",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicSchoolSchedules");

            migrationBuilder.DropTable(
                name: "Relationships");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Guardians");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Guardians");

            migrationBuilder.RenameColumn(
                name: "Distance",
                table: "Ratings",
                newName: "SchoolLevel");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<string>(
                name: "Relationship",
                table: "Guardians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Guardians",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Guardians_StudentId",
                table: "Guardians",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guardians_Students_StudentId",
                table: "Guardians",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
