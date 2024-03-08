using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IRequestWiseFilesRepository:IGenericRepository<RequestWiseFile>
    {
        Task<RequestWiseFile> FindFile(string fileName);
        Task<RequestWiseFile> FindFileByID(string fileName, int? ID);
        IQueryable<RequestWiseFile> FindFileByRequestID(int? requestID);
        Task<List<RequestWiseFile>> GetAllByRequestId(int Id);
        Task DeleteByRequestIdAndFileName( string fileName);
    }
}
