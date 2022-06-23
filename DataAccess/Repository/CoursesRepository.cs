using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CoursesRepository : Repository<Course>, ICoursesRepository
    {
        private readonly ApplicationDbContext _db;
        public CoursesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Course obj)
        {
            _db.Courses.Update(obj);
        }
    }
}
