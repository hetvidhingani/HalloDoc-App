using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RequestWiseFilesRepository: GenericRepository<RequestWiseFile>, IRequestWiseFilesRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestWiseFilesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
       
      
    }
}
