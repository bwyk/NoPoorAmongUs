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


        }

        public IApplicantsRepository Applicant { get; private set; }

        public IClassesRepository Class { get; private set; }

        public ICoursesRepository Course { get; private set; }

        public IEmployeesRepository Employee { get; private set; }

        public IGuardiansRepository Guardian { get; private set; }

        public IRatingsRepository Rating { get; private set; }

        public IStudentsRepository Student { get; private set; }

        public ITermsRepository Term { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
