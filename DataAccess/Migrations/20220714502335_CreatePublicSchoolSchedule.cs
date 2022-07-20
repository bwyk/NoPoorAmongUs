using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class CreatePublicSchoolSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "PublicSchoolSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    Monday = table.Column<bool>(type: "bit", nullable: false),
                    Tuesday = table.Column<bool>(type: "bit", nullable: false),
                    Wednesday = table.Column<bool>(type: "bit", nullable: false),
                    Thursday = table.Column<bool>(type: "bit", nullable: false),
                    Friday = table.Column<bool>(type: "bit", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicSchoolSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicSchoolSchedules_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicSchoolSchedules_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicSchoolSchedules_SchoolId",
                table: "PublicSchoolSchedules",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicSchoolSchedules_StudentId",
                table: "PublicSchoolSchedules",
                column: "StudentId");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_CourseEnrollments_CourseEnrollmentId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentDocs_Students_StudentId",
                table: "StudentDocs");

            migrationBuilder.DropTable(
                name: "PublicSchoolSchedules");

            migrationBuilder.DropIndex(
                name: "IX_StudentDocs_StudentId",
                table: "StudentDocs");

            migrationBuilder.DropColumn(
                name: "DocUrl",
                table: "StudentDocs");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "StudentDocs");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Assessments");

            migrationBuilder.RenameColumn(
                name: "CourseEnrollmentId",
                table: "Grades",
                newName: "ClassEnrollmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_CourseEnrollmentId",
                table: "Grades",
                newName: "IX_Grades_ClassEnrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_CourseEnrollments_ClassEnrollmentId",
                table: "Grades",
                column: "ClassEnrollmentId",
                principalTable: "CourseEnrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
