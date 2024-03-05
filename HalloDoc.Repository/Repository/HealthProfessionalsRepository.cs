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
    public class HealthProfessionalsRepository:GenericRepository<HealthProfessional>,IHealthProfessionalsRepository
    {
        private readonly ApplicationDbContext _context;
        public HealthProfessionalsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<HealthProfessional>> GetVendor()
        {
            return _context.HealthProfessionals.ToList();
        }
        public async Task<List<HealthProfessional>> GetBusinessByProfession(int professionId)
        {
            var Professions = await _context.HealthProfessionals
                .Where(p => p.Profession == professionId)
                .ToListAsync();

            return Professions;
        }
    }
}
