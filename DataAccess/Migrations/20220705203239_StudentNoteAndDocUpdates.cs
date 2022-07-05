using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class StudentNoteAndDocUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocUrl",
                table: "StudentDocs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "StudentDocs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "DocTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocs_StudentId",
                table: "StudentDocs",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDocs_Students_StudentId",
                table: "StudentDocs",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentDocs_Students_StudentId",
                table: "StudentDocs");

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
                name: "Type",
                table: "DocTypes");
        }
    }
}
