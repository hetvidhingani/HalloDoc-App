using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RequestStatusLogRepository : GenericRepository<RequestStatusLog> ,IRequestStatusLogRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestStatusLogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<RequestStatusLog> CheckByRequestID(int id)
        {
            RequestStatusLog user = _context.RequestStatusLogs.Where(x => x.RequestId == id).FirstOrDefault();
            return user;
        }

    }
}
