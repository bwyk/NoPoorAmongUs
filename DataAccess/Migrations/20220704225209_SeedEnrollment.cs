using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class SeedEnrollment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_ClassEnrollments_ClassEnrollmentId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassEnrollments_ClassSessions_CourseSessionId",
                table: "ClassEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassEnrollments_Students_StudentId",
                table: "ClassEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSessions_Courses_CourseId",
                table: "ClassSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_ClassEnrollments_ClassEnrollmentId",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassSessions",
                table: "ClassSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassEnrollments",
                table: "ClassEnrollments");

            migrationBuilder.RenameTable(
                name: "ClassSessions",
                newName: "CourseSessions");

            migrationBuilder.RenameTable(
                name: "ClassEnrollments",
                newName: "CourseEnrollments");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSessions_CourseId",
                table: "CourseSessions",
                newName: "IX_CourseSessions_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassEnrollments_StudentId",
                table: "CourseEnrollments",
                newName: "IX_CourseEnrollments_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassEnrollments_CourseSessionId",
                table: "CourseEnrollments",
                newName: "IX_CourseEnrollments_CourseSessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSessions",
                table: "CourseSessions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseEnrollments",
                table: "CourseEnrollments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_CourseEnrollments_ClassEnrollmentId",
                table: "Attendances",
                column: "ClassEnrollmentId",
                principalTable: "CourseEnrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEnrollments_CourseSessions_CourseSessionId",
                table: "CourseEnrollments",
                column: "CourseSessionId",
                principalTable: "CourseSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEnrollments_Students_StudentId",
                table: "CourseEnrollments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSessions_Courses_CourseId",
                table: "CourseSessions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_CourseEnrollments_ClassEnrollmentId",
                table: "Grades",
                column: "ClassEnrollmentId",
                principalTable: "CourseEnrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_CourseEnrollments_ClassEnrollmentId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseEnrollments_CourseSessions_CourseSessionId",
                table: "CourseEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseEnrollments_Students_StudentId",
                table: "CourseEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSessions_Courses_CourseId",
                table: "CourseSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_CourseEnrollments_ClassEnrollmentId",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSessions",
                table: "CourseSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseEnrollments",
                table: "CourseEnrollments");

            migrationBuilder.RenameTable(
                name: "CourseSessions",
                newName: "ClassSessions");

            migrationBuilder.RenameTable(
                name: "CourseEnrollments",
                newName: "ClassEnrollments");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSessions_CourseId",
                table: "ClassSessions",
                newName: "IX_ClassSessions_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseEnrollments_StudentId",
                table: "ClassEnrollments",
                newName: "IX_ClassEnrollments_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseEnrollments_CourseSessionId",
                table: "ClassEnrollments",
                newName: "IX_ClassEnrollments_CourseSessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassSessions",
                table: "ClassSessions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassEnrollments",
                table: "ClassEnrollments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_ClassEnrollments_ClassEnrollmentId",
                table: "Attendances",
                column: "ClassEnrollmentId",
                principalTable: "ClassEnrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassEnrollments_ClassSessions_CourseSessionId",
                table: "ClassEnrollments",
                column: "CourseSessionId",
                principalTable: "ClassSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassEnrollments_Students_StudentId",
                table: "ClassEnrollments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSessions_Courses_CourseId",
                table: "ClassSessions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_ClassEnrollments_ClassEnrollmentId",
                table: "Grades",
                column: "ClassEnrollmentId",
                principalTable: "ClassEnrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
