﻿using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class BusinessRepository:GenericRepository<Business>,IBusinessRepository
    {
        private readonly ApplicationDbContext _context;

        public BusinessRepository(ApplicationDbContext context) : base(context)
        { 
            _context = context;

        }
    }
}
