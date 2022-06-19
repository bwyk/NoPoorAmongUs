using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class StudentsRepository : Repository<Student>, IStudentsRepository
    {
        private readonly ApplicationDbContext _db;
        public StudentsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Student obj)
        {
            _db.Students.Update(obj);
        }
    }
}
