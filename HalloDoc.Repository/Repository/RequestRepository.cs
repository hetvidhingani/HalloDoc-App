using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RequestRepository:IRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddRequest(Request request)
        {
            _context.Add(request);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRequest(Request request)
        {
            _context.Update(request);
            await _context.SaveChangesAsync();
        }
        public async Task<Request> CheckUserByEmail(string email)
        {
            Request userByRequest = _context.Requests.Where(x => x.Email == email).FirstOrDefault();

            return userByRequest;
        }
    }
}
