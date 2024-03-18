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
    public interface IPatientService
    {
        Task<object> RegionList();
        Task<object> RegionListUser();
        Task<object> PatientRequest(int? userId);
        Task<string> PatientRequest(PatientRequestViewModel viewModel);
        Task<string> FamilyFriendRequest(OtherRequestViewModel viewModel);
        Task<string> BusinessRequest(OtherRequestViewModel viewModel);
        Task<string> ConciergeRequest(OtherRequestViewModel viewModel);
        string? CheckEmail(string email);
        Task<AspNetUser> checkEmailPassword(string email, string password);
        Task<User> GetUser(string email);
        //Task<AspNetUser> GetAspNetUser(string email);
        Task<string> PatientForgotPassword(CreateAccountViewModel createAccountViewModel);
        Task<T> GetTempData<T>(string key);
        Task<object> Dashboard(int? userId);
        Task<string> SubmitInformationSomeoneElse(PatientRequestViewModel viewModel,int? userId);
        Task<string> CreateAccountRequest(CreateAccountViewModel createAccountViewModel);
        Task<object> Profile(PatientRequestViewModel requestViewModel, int? userId);
        Task<User> EditUser(PatientRequestViewModel patientRequestViewModel, int? userId);
        Task<List<DashboardViewModel>> ViewDocument(int Id);
        Task<string> ViewDocument(IFormFile a, int Id);
       // Task<RequestWiseFile> FindFile(string email);
        Task<RequestWiseFile> DownloadFile(int file);
        Task<byte[]> DownloadAllByChecked(IEnumerable<int> documentValues);
        Task<byte[]> DownloadAll(IEnumerable<int> documentValues, int? requestid);
        Task<string> SendEmail(string email, string link, string subject, string body);
      

    }
}
