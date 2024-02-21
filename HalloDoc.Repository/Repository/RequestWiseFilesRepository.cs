using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RequestWiseFilesRepository:IRequestWiseFilesRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestWiseFilesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddRequestWiseFiles(RequestWiseFile requestWiseFile)
        {
            _context.Add(requestWiseFile);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRequestWiseFiles(RequestWiseFile requestWiseFile)
        {
            _context.Update(requestWiseFile);
            await _context.SaveChangesAsync();
        }
      
    }
}
