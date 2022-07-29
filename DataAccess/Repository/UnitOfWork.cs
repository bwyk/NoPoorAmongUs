using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Course = new CoursesRepository(_db);
            Student = new StudentsRepository(_db);
            Guardian = new GuardiansRepository(_db);
            Rating = new RatingsRepository(_db);
            Term = new TermsRepository(_db);
            Assessment = new AssessmentsRepository(_db);
            Attendance = new AttendanceRepository(_db);
            CourseEnrollment = new CourseEnrollmentRepository(_db);
            CourseSession = new CourseSessionsRepository(_db);
            DocType = new DocTypeRepository(_db);
            Grade = new GradeRepository(_db);
            NoteType = new NoteTypeRepository(_db);
            School = new SchoolRepository(_db);
            StudentDoc = new StudentDocRepository(_db);
            StudentNote = new StudentNoteRepository(_db);
            Subject = new SubjectRepository(_db);
            PublicSchoolSchedules = new PublicSchoolScheduleRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            Relationship = new RelationshipRepository(_db);
        }

        public IPublicSchoolScheduleRepository PublicSchoolSchedules { get; private set; }
        public ICoursesRepository Course { get; private set; }
        public IGuardiansRepository Guardian { get; private set; }
        public IRatingsRepository Rating { get; private set; }
        public IStudentsRepository Student { get; private set; }
        public ITermsRepository Term { get; private set; }
        public IAssessmentsRepository Assessment { get; private set; }
        public IAttendanceRepository Attendance { get; private set; }
        public ICourseEnrollmentsRepository CourseEnrollment { get; private set; }
        public ICourseSessionsRepository CourseSession { get; private set; }
        public IDocTypeRepository DocType { get; private set; }
        public IGradeRepository Grade { get; private set; }
        public INoteTypeRepository NoteType { get; private set; }
        public ISchoolRepository School { get; private set; }
        public IStudentDocRepository StudentDoc { get; private set; }
        public IStudentNoteRepository StudentNote { get; private set; }
        public ISubjectRepository Subject { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IRelationshipRepository Relationship { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
