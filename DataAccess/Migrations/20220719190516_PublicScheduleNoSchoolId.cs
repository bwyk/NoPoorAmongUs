using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class PublicScheduleNoSchoolId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicSchoolSchedules_Schools_SchoolId",
                table: "PublicSchoolSchedules");

            migrationBuilder.DropIndex(
                name: "IX_PublicSchoolSchedules_SchoolId",
                table: "PublicSchoolSchedules");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "PublicSchoolSchedules");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseEnrollments_CourseSessions_CourseSessionId",
                table: "CourseEnrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSessions",
                table: "CourseSessions");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "CourseSessions");

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "PublicSchoolSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TempId",
                table: "CourseSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CourseSessions_TempId",
                table: "CourseSessions",
                column: "TempId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicSchoolSchedules_SchoolId",
                table: "PublicSchoolSchedules",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEnrollments_CourseSessions_CourseSessionId",
                table: "CourseEnrollments",
                column: "CourseSessionId",
                principalTable: "CourseSessions",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicSchoolSchedules_Schools_SchoolId",
                table: "PublicSchoolSchedules",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
