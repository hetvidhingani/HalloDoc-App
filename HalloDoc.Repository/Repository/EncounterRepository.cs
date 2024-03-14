using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class EncounterRepository : GenericRepository<Encounter>, IEncounterRepository
    {
        private readonly ApplicationDbContext _context;
        public EncounterRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Encounter> checkBYRequestID(int requestId)
        {
            var encounter =  _context.Encounters.Where(u=> u.Requestid == requestId).FirstOrDefault();
            return encounter;
        }
    }
}
