using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class PublicSchoolScheduleRepository : Repository<PublicSchoolSchedule>, IPublicSchoolScheduleRepository
    {
        private readonly ApplicationDbContext _db;
        public PublicSchoolScheduleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PublicSchoolSchedule obj)
        {
            _db.PublicSchoolSchedules.Update(obj);
        }
    }
}
