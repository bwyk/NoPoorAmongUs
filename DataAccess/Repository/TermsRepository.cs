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
    public class TermsRepository : Repository<Term>, ITermsRepository
    {
        private readonly ApplicationDbContext _db;
        public TermsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Term obj)
        {
            _db.Terms.Update(obj);
        }
    }
}
