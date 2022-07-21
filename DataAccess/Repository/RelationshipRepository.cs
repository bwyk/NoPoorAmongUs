using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RelationshipRepository : Repository<Relationship>, IRelationshipRepository
    {
        private readonly ApplicationDbContext _db;
        public RelationshipRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Relationship obj)
        {
            _db.Relationships.Update(obj);
        }
    }
}