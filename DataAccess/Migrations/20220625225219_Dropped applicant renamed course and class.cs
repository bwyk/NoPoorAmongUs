using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class Droppedapplicantrenamedcourseandclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guardians_Applicants_ApplicantId",
                table: "Guardians");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Applicants_ApplicantId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Applicants_ApplicantId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Students_ApplicantId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Students",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ApplicantId",
                table: "Ratings",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_ApplicantId",
                table: "Ratings",
                newName: "IX_Ratings_StudentId");

            migrationBuilder.RenameColumn(
                name: "ApplicantId",
                table: "Guardians",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Guardians_ApplicantId",
                table: "Guardians",
                newName: "IX_Guardians_StudentId");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Guardians_Students_StudentId",
                table: "Guardians",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Students_StudentId",
                table: "Ratings",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guardians_Students_StudentId",
                table: "Guardians");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Students_StudentId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Students",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Ratings",
                newName: "ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_StudentId",
                table: "Ratings",
                newName: "IX_Ratings_ApplicantId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Guardians",
                newName: "ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_Guardians_StudentId",
                table: "Guardians",
                newName: "IX_Guardians_ApplicantId");

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Village = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_ApplicantId",
                table: "Students",
                column: "ApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guardians_Applicants_ApplicantId",
                table: "Guardians",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Applicants_ApplicantId",
                table: "Ratings",
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
    }
}
