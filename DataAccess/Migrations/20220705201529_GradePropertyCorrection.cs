using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class GradePropertyCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_CourseEnrollments_ClassEnrollmentId",
                table: "Grades");

            migrationBuilder.RenameColumn(
                name: "ClassEnrollmentId",
                table: "Grades",
                newName: "CourseEnrollmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_ClassEnrollmentId",
                table: "Grades",
                newName: "IX_Grades_CourseEnrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_CourseEnrollments_CourseEnrollmentId",
                table: "Grades",
                column: "CourseEnrollmentId",
                principalTable: "CourseEnrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_CourseEnrollments_CourseEnrollmentId",
                table: "Grades");

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
