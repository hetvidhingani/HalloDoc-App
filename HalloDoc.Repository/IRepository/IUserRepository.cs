using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IUserRepository
    {
        public Task AddUser(User user);

        public Task UpdateUser(User user);
        public Task<User> CheckUserByEmail(string email);
        public Task<User> GetUser(int? userID);

    }
}
