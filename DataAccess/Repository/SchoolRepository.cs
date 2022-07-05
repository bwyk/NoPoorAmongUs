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
    public class SchoolRepository : Repository<School>, ISchoolRepository
    {
        private readonly ApplicationDbContext _db;
        public SchoolRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(School obj)
        {
            _db.Schools.Update(obj);
        }
    }
}
