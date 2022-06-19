using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicantsRepository Applicant { get; }
        IClassesRepository Class { get; }
        ICoursesRepository Course { get; }
        IEmployeesRepository Employee { get; }
        IGuardiansRepository Guardian { get; }
        IPersonsRepository Person { get; }
        IRatingsRepository Rating { get; }
        IStudentsRepository Student { get; }
        ITermsRepository Term { get; }
        void Save();
    }
}
