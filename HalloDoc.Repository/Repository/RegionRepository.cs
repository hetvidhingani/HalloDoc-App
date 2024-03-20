using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RegionRepository:GenericRepository<Region>,IRegionRepository
    {
        private readonly ApplicationDbContext _context;
        public RegionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Region>> GetRegions()
        {
            return _context.Regions.ToList();
        }
        public async Task<string> FindState(int? regionID)
        {
            Region state = _context.Regions.Where(u => u.RegionId == regionID).FirstOrDefault();

            return state.Name;
        }
    }
}
