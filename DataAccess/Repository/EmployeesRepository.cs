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
    public class EmployeesRepository : Repository<Instructor>, IEmployeesRepository
    {
        private readonly ApplicationDbContext _db;
        public EmployeesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Instructor obj)
        {
            _db.Employees.Add(obj);
        }
    }
}
