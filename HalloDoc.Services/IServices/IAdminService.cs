using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.IServices
{

    public interface IAdminService
    {
        Task<AspNetUser> checkEmailPassword(AspNetUser user);
        Task<User> GetUser(string email);
       
        List<AdminDashboardViewModel> New();
    }
}
