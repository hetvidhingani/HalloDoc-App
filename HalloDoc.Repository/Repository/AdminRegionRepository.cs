using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class AdminRegionRepository:GenericRepository<AdminRegion>,IAdminRegionRepository
    {
        private readonly ApplicationDbContext _context;
        public AdminRegionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<AdminRegion>> adminRegionByAdminID(int adminID)
        {
            var result = _context.AdminRegions.Where(u => u.AdminId == adminID).ToList();
            return result;
        }
        
    }
}
