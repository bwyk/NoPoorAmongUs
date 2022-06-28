using DataAccess.Data;
using DataAccess.Repository.IRepository;
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
        }

        public IClassesRepository Class { get; private set; }
        public ICoursesRepository Course { get; private set; }
        public IInstructorRepository Employee { get; private set; }
        public IGuardiansRepository Guardian { get; private set; }
        public IRatingsRepository Rating { get; private set; }
        public IStudentsRepository Student { get; private set; }
        public ITermsRepository Term { get; private set; }
        public IAssessmentsRepository Assessment { get; private set; }
        public IAttendanceRepository Attendance { get; private set; }
        public IClassEnrollmentsRepository ClassEnrollment { get; private set; }
        public IClassSessionsRepository ClassSession { get; private set; }
        public IDocTypeRepository DocType { get; private set; }
        public IGradeRepository Grade { get; private set; }
        public INoteTypeRepository NoteType { get; private set; }
        public ISchoolRepository School { get; private set; }
        public IStudentDocRepository StudentDoc { get; private set; }
        public IStudentNoteRepository StudentNote { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
