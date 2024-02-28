﻿using HalloDoc.Entities.DataModels;
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
        List<AdminDashboardViewModel> Pending();
        List<AdminDashboardViewModel> Active();
        List<AdminDashboardViewModel> Conclude();
        List<AdminDashboardViewModel> ToClose();
        List<AdminDashboardViewModel> Unpaid();
        Task<int> GetCount(int statusId);


    }
}