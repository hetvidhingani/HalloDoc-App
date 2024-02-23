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
    public class RequestWiseFilesRepository: GenericRepository<RequestWiseFile>, IRequestWiseFilesRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestWiseFilesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RequestWiseFile> FindFile(string fileName)
        {
            RequestWiseFile file = _context.RequestWiseFiles.Where(x => x.FileName == fileName).FirstOrDefault();

            return file;
        }
        public IQueryable<RequestWiseFile> FindFileByRequestID(int? requestID)
        {
            return _context.RequestWiseFiles.Where(x => x.RequestId == requestID);
        }
    }
}
