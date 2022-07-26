using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class GradeComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "StudentNotes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StudentNotes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "StudentNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "NoteTypes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Grades",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotes_ApplicationUserId",
                table: "StudentNotes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTypes_RoleId",
                table: "NoteTypes",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTypes_AspNetRoles_RoleId",
                table: "NoteTypes",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentNotes_AspNetUsers_ApplicationUserId",
                table: "StudentNotes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteTypes_AspNetRoles_RoleId",
                table: "NoteTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentNotes_AspNetUsers_ApplicationUserId",
                table: "StudentNotes");

            migrationBuilder.DropIndex(
                name: "IX_StudentNotes_ApplicationUserId",
                table: "StudentNotes");

            migrationBuilder.DropIndex(
                name: "IX_NoteTypes_RoleId",
                table: "NoteTypes");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "StudentNotes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StudentNotes");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "StudentNotes");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "NoteTypes");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Grades");
        }
    }
}
