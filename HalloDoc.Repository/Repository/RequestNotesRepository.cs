using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RequestNotesRepository:GenericRepository<RequestNote>,IRequestNotesRepository
    {
      
            private readonly ApplicationDbContext _context;
            public RequestNotesRepository(ApplicationDbContext context) : base(context)
            {
                _context = context;
            }
           public async Task<RequestNote> CheckByRequestID(int id)
        {
            RequestNote user = _context.RequestNotes.Where(x => x.RequestId == id).FirstOrDefault();
            return user;
        }


    }
}
