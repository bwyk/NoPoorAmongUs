using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class FurtherModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Employees_InstructorId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_School_SchoolId",
                table: "Classes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_School",
                table: "School");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "School",
                newName: "Schools");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Instructors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schools",
                table: "Schools",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instructors",
                table: "Instructors",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSessions_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoteTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassEnrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassSessionId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassEnrollments_ClassSessions_ClassSessionId",
                        column: x => x.ClassSessionId,
                        principalTable: "ClassSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassEnrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentDocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDocs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentDocs_DocTypes_DocTypeId",
                        column: x => x.DocTypeId,
                        principalTable: "DocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    NoteTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentNotes_NoteTypes_NoteTypeId",
                        column: x => x.NoteTypeId,
                        principalTable: "NoteTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentNotes_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassEnrollmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_ClassEnrollments_ClassEnrollmentId",
                        column: x => x.ClassEnrollmentId,
                        principalTable: "ClassEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Score = table.Column<int>(type: "int", nullable: false),
                    AssessmentId = table.Column<int>(type: "int", nullable: false),
                    ClassEnrollmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grades_ClassEnrollments_ClassEnrollmentId",
                        column: x => x.ClassEnrollmentId,
                        principalTable: "ClassEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ClassId",
                table: "Assessments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassEnrollmentId",
                table: "Attendances",
                column: "ClassEnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_ClassSessionId",
                table: "ClassEnrollments",
                column: "ClassSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_StudentId",
                table: "ClassEnrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessions_ClassId",
                table: "ClassSessions",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_AssessmentId",
                table: "Grades",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ClassEnrollmentId",
                table: "Grades",
                column: "ClassEnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocs_DocTypeId",
                table: "StudentDocs",
                column: "DocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotes_NoteTypeId",
                table: "StudentNotes",
                column: "NoteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotes_StudentId",
                table: "StudentNotes",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Instructors_InstructorId",
                table: "Classes",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Schools_SchoolId",
                table: "Classes",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Instructors_InstructorId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Schools_SchoolId",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "StudentDocs");

            migrationBuilder.DropTable(
                name: "StudentNotes");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "ClassEnrollments");

            migrationBuilder.DropTable(
                name: "DocTypes");

            migrationBuilder.DropTable(
                name: "NoteTypes");

            migrationBuilder.DropTable(
                name: "ClassSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schools",
                table: "Schools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Instructors",
                table: "Instructors");

            migrationBuilder.RenameTable(
                name: "Schools",
                newName: "School");

            migrationBuilder.RenameTable(
                name: "Instructors",
                newName: "Employees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_School",
                table: "School",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

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
        }
    }
}
