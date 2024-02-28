﻿using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class PhysicianRepository:GenericRepository<Physician>,IPhysicianRepository
    {
        private readonly ApplicationDbContext _context;

        public PhysicianRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }
    }
}