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
        Task<object> RegionListUser(PatientRequestViewModel viewModel);
        Task<object> PatientRequest(int? userId);
        Task<string> PatientRequest(PatientRequestViewModel viewModel);
        Task<string> FamilyFriendRequest(OtherRequestViewModel viewModel);
        Task<string> BusinessRequest(OtherRequestViewModel viewModel);
        Task<string> ConciergeRequest(OtherRequestViewModel viewModel);
        string? CheckEmail(string email);
 
        Task<User> GetUser(string email);
       
       
        Task<T> GetTempData<T>(string key);
        Task<object> Dashboard(int? userId);
        Task<string> SubmitInformationSomeoneElse(PatientRequestViewModel viewModel,int? userId);
        Task<string> CreateAccountRequest(CreateAccountViewModel createAccountViewModel);
        Task<object> Profile(PatientRequestViewModel requestViewModel, int? userId);
        Task<User> EditUser(PatientRequestViewModel patientRequestViewModel, int? userId);
   



    }
}
