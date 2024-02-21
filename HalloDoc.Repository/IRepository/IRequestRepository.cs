using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IRequestRepository
    {
        public Task AddRequest(Request request);

        public Task UpdateRequest(Request request);
        public Task<Request> CheckUserByEmail(string email);

    }
}
