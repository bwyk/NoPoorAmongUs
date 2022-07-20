using Models;
using Models.Academic;
using Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IRelationshipRepository : IRepository<Relationship>
    {
        void Update(Relationship obj);
    }
}