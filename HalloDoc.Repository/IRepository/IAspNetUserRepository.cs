using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IAspNetUserRepository:IGenericRepository<AspNetUser>
    {

        Task<AspNetUser> CheckUserByEmail(string email);
        Task<AspNetUser> Login(string email,string password);
        Task<AspNetUser> getById(string ID);
    }
}
