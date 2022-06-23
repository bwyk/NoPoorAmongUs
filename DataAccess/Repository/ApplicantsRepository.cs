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
    public class ApplicantsRepository : Repository<Applicant>, IApplicantsRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicantsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Applicant obj)
        {
           _db.Applicants.Add(obj);
        }
    }
}