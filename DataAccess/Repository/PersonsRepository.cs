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
    public class PersonsRepository : Repository<Person>, IPersonsRepository
    {
        private readonly ApplicationDbContext _db;
        public PersonsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Person obj)
        {
            _db.Persons.Update(obj);
        }
    }
}
