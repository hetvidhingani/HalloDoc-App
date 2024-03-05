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
    public class HealthProfessionalTypeRepository:GenericRepository<HealthProfessionalType>,IHealthProfessionalTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public HealthProfessionalTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<HealthProfessionalType>> GetProfession()
        {
            return _context.HealthProfessionalTypes.ToList();
        }
    }
}
