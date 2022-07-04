using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class ClassEnrollment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassEnrollments_ClassSessions_ClassSessionId",
                table: "ClassEnrollments");

            migrationBuilder.RenameColumn(
                name: "ClassSessionId",
                table: "ClassEnrollments",
                newName: "CourseSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassEnrollments_ClassSessionId",
                table: "ClassEnrollments",
                newName: "IX_ClassEnrollments_CourseSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassEnrollments_ClassSessions_CourseSessionId",
                table: "ClassEnrollments",
                column: "CourseSessionId",
                principalTable: "ClassSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassEnrollments_ClassSessions_CourseSessionId",
                table: "ClassEnrollments");

            migrationBuilder.RenameColumn(
                name: "CourseSessionId",
                table: "ClassEnrollments",
                newName: "ClassSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassEnrollments_CourseSessionId",
                table: "ClassEnrollments",
                newName: "IX_ClassEnrollments_ClassSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassEnrollments_ClassSessions_ClassSessionId",
                table: "ClassEnrollments",
                column: "ClassSessionId",
                principalTable: "ClassSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
