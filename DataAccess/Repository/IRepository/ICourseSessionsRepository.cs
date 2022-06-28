using Models;
using Models.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface ICourseSessionsRepository : IRepository<ClassSession>
    {
        void Update(ClassSession obj);
    }
}
