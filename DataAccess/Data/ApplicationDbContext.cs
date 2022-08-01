using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Models.Academic;
using Models.People;
namespace DataAccess.Data
{

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

            //  We are "using" the EntityFramework(which supports several data sources like Oracle, MS SQL Server, MySQL) to "map" our local models to physical database tables

            //We register the  DBContextOptions as a "service" in the "build" pipeline within the Program.cs file 
        }
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<Rating> Ratings => Set<Rating>();
        public DbSet<Term> Terms => Set<Term>();
        public DbSet<Guardian> Guardians => Set<Guardian>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Assessment> Assessments => Set<Assessment>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<SessionAttendance> SessionAttendances => Set<SessionAttendance>();
        public DbSet<CourseEnrollment> CourseEnrollments => Set<CourseEnrollment>();
        public DbSet<CourseSession> CourseSessions => Set<CourseSession>();
        public DbSet<DocType> DocTypes => Set<DocType>();
        public DbSet<Grade> Grades => Set<Grade>();
        public DbSet<NoteType> NoteTypes => Set<NoteType>();
        public DbSet<School> Schools => Set<School>();
        public DbSet<StudentDoc> StudentDocs => Set<StudentDoc>();
        public DbSet<StudentNote> StudentNotes => Set<StudentNote>();
        public DbSet<PublicSchoolSchedule> PublicSchoolSchedules => Set<PublicSchoolSchedule>();
        public DbSet<ApplicationUser> ApplicationUser => Set<ApplicationUser>();
        public DbSet<Relationship> Relationships => Set<Relationship>();

    }
}
