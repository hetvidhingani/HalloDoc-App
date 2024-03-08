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
            return _context.RequestWiseFiles.Where(x => x.RequestId == requestID && x.IsDeleted == null);
        }
        
        public async Task<List<RequestWiseFile>> GetAllByRequestId(int Id)
        {
            return await _context.RequestWiseFiles.Where(x => x.RequestId == Id).ToListAsync();
        }
        public async Task DeleteByRequestIdAndFileName( string fileName)
        {
            var file = await _context.RequestWiseFiles.FirstOrDefaultAsync(f =>  f.FileName == fileName);
            if (file != null)
            {
                _context.RequestWiseFiles.Remove(file);
                await _context.SaveChangesAsync();
            }
        }
    }
}
