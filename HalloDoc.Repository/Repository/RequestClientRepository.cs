using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RequestClientRepository : GenericRepository<RequestClient>,IRequestClientRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestClientRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }
        public async Task<RequestClient> CheckUserByEmail(string email)
        {
            RequestClient user = _context.RequestClients.Where(x => x.Email == email).FirstOrDefault();

            return user;
        }
        public async Task<RequestClient> CheckUserByID(int id)
        {
            RequestClient user = _context.RequestClients.Where(x => x.RequestClientId == id).FirstOrDefault();

            return user;
        }
        public async Task<RequestClient> GetRequestIDByClientID(int id)
        {
            RequestClient user = _context.RequestClients.Where(x => x.RequestClientId == id).FirstOrDefault();

            return user;
        }
    }
}
