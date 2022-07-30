using Models;
using Models.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface INoteTypeRepository : IRepository<NoteType>
    {
        void Update(NoteType obj);
        void UpdateRange(IEnumerable<NoteType> entities);
    }
}
