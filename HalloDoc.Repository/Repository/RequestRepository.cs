using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RequestRepository: GenericRepository<Request>, IRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestRepository(ApplicationDbContext context):base(context)
        {
           _context = context;
        }
        public async Task<Request> CheckUserByEmail(string email)
        {
            Request user = _context.Requests.Where(x => x.Email == email).FirstOrDefault();

            return user;
        }
    }
}
