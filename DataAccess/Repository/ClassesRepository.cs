﻿using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ClassesRepository : Repository<Class>, IClassesRepository
    {
        private readonly ApplicationDbContext _db;
        public ClassesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Class obj)
        {
            _db.Classes.Update(obj);
        }
    }
}