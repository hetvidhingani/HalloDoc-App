using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class CaseTagRepository : GenericRepository<CaseTag>, ICaseTagRepository
    {
        private readonly ApplicationDbContext _context;
        public CaseTagRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<CaseTag>> GetCases()
        {
            return  _context.CaseTags.ToList();
        }
    }
}
