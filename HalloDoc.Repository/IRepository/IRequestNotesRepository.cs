using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IRequestNotesRepository:IGenericRepository<RequestNote>
    {
        Task<RequestNote> CheckByRequestID(int id);
    }
}
