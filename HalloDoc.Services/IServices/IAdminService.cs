using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
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
        Task<T> GetTempData<T>(string key);
        List<AdminDashboardViewModel> New();
        List<AdminDashboardViewModel> Pending();
        List<AdminDashboardViewModel> Active();
        List<AdminDashboardViewModel> Conclude();
        List<AdminDashboardViewModel> ToClose();
        List<AdminDashboardViewModel> Unpaid();
        Task<int> GetCount(int statusId);
        Task<object> ViewCase( int userId);
        Task<int> GetUserByRequestClientID(int id);
        Task<Admin> GetAdmin(string email);
        Task<string> EditNewRequest(ViewCaseViewModel viewModel, int? userId);
        Task<object> ViewNotes(AdminDashboardViewModel viewmodel, int id);
        Task<object> AddNotes(AdminDashboardViewModel viewmodel, int id);
        Task<object> CancelCase(CancelCaseViewModel viewModel, int id);
        Task<string> ConfirmCancelCase(CancelCaseViewModel viewModel, int id);
        Task<object> AssignCase(AssignCaseViewModel viewModel, int id);
        Task<string> AssignRequest(AssignCaseViewModel viewModel, int id);
        Task<object> BlockCase(CancelCaseViewModel viewModel, int id);
        Task<string> BlockCaseRequest(CancelCaseViewModel viewModel, int id);
        Task<List<Physician>> GetPhysiciansByRegion(int regionId);
        Task<DashboardViewModel> ViewDocument(int Id);
        Task<string> ViewDocument(IFormFile a, int Id);
        Task<RequestWiseFile> FindFile(string email);
        Task<RequestWiseFile> FindFile(string fileName, int? ID);
        Task<RequestWiseFile> DownloadFile(string name);
        Task<byte[]> DownloadAllByChecked(IEnumerable<string> selectedFiles);
        Task<byte[]> DownloadAll(IEnumerable<string> selectedFiles, int? requestid);
        Task<object> DeleteFile(int name);
        Task<object> SendOrder(SendOrderViewModel viewModel, int Id);
        Task<List<HealthProfessional>> GetBusinessByProfession(int professionId);
        Task<HealthProfessional> GetBusinessDetails(object BusinessId);
        Task<string> SendOrderDetails(SendOrderViewModel viewModel, int id);
    }
}
