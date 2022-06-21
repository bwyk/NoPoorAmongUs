using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class ModelSkeleton : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Ratings_RatingsId",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Employees_EmployeeId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Guardians_Applicants_GuardianId",
                table: "Guardians");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_ClassId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_ClassId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_RatingsId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "School",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "RatingsId",
                table: "Applicants");

            migrationBuilder.RenameColumn(
                name: "GuardianId",
                table: "Guardians",
                newName: "ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_Guardians_GuardianId",
                table: "Guardians",
                newName: "IX_Guardians_ApplicantId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Classes",
                newName: "SchoolId");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_EmployeeId",
                table: "Classes",
                newName: "IX_Classes_SchoolId");

            migrationBuilder.AddColumn<int>(
                name: "ApplicantId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_ApplicantId",
                table: "Students",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CourseId",
                table: "Classes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_InstructorId",
                table: "Classes",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Courses_CourseId",
                table: "Classes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Employees_InstructorId",
                table: "Classes",
                column: "InstructorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_School_SchoolId",
                table: "Classes",
                column: "SchoolId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Guardians_Applicants_ApplicantId",
                table: "Guardians",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Applicants_ApplicantId",
                table: "Students",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Courses_CourseId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Employees_InstructorId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_School_SchoolId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Guardians_Applicants_ApplicantId",
                table: "Guardians");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Applicants_ApplicantId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropIndex(
                name: "IX_Students_ApplicantId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Classes_CourseId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_InstructorId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "ApplicantId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "ApplicantId",
                table: "Guardians",
                newName: "GuardianId");

            migrationBuilder.RenameIndex(
                name: "IX_Guardians_ApplicantId",
                table: "Guardians",
                newName: "IX_Guardians_GuardianId");

            migrationBuilder.RenameColumn(
                name: "SchoolId",
                table: "Classes",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_SchoolId",
                table: "Classes",
                newName: "IX_Classes_EmployeeId");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RatingsId",
                table: "Applicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_RatingsId",
                table: "Applicants",
                column: "RatingsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Ratings_RatingsId",
                table: "Applicants",
                column: "RatingsId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Employees_EmployeeId",
                table: "Classes",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Guardians_Applicants_GuardianId",
                table: "Guardians",
                column: "GuardianId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_ClassId",
                table: "Students",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");
        }
    }
}
