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
    public class RatingsRepository : Repository<Ratings>, IRatingsRepository
    {
        private readonly ApplicationDbContext _db;
        public RatingsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Ratings obj)
        {
            _db.Ratings.Update(obj);
        }
    }
}
