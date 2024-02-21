using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RequestClientRepository : IRequestClientRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddRequest(RequestClient requestClient)
        {
            _context.Add(requestClient);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRequest(RequestClient requestClient)
        {
            _context.Update(requestClient);
            await _context.SaveChangesAsync();
        }
    }
}
