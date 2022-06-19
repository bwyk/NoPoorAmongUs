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
    public class GuardiansRepository : Repository<Guardian>, IGuardiansRepository
    {
        private readonly ApplicationDbContext _db;
        public GuardiansRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Guardian obj)
        {
           _db.Guardians.Update(obj);
        }
    }
}
