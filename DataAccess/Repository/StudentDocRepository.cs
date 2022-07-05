using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using Models.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class StudentDocRepository : Repository<StudentDoc>, IStudentDocRepository
    {
        private readonly ApplicationDbContext _db;
        public StudentDocRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(StudentDoc obj)
        {
            _db.StudentDocs.Update(obj);
        }
    }
}
