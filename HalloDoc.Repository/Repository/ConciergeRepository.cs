using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class ConciergeRepository:GenericRepository<Concierge>,IConciergeRepository
    {
        private readonly ApplicationDbContext _context;

        public ConciergeRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }
       
    }
}
