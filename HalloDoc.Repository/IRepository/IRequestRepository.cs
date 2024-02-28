using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IRequestRepository:IGenericRepository<Request>
    {
        Task<Request> CheckUserByEmail(string email);
        Task<int> GetCountAsync(Expression<Func<Request, bool>> predicate);
    }
}
