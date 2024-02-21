using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IAspNetUserRepository
    {
        public Task AddAspnetUser(AspNetUser aspnetuser);

        public Task UpdateAspnetUser(AspNetUser aspnetuser);
        public Task<AspNetUser> CheckUserByEmail(string email);
        public Task<AspNetUser> CheckUserForLogin(string email, string password);
    }
}
