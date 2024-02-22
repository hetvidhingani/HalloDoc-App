using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.IServices
{
    public interface IPatientService
    {
        Task<object> PatientRequest(int? userId);
        Task<string> PatientRequest(PatientRequestViewModel viewModel);
        Task<string> FamilyFriendRequest(OtherRequestViewModel viewModel);
        Task<string> BusinessRequest(OtherRequestViewModel viewModel);
        Task<string> ConciergeRequest(OtherRequestViewModel viewModel);
        Task<AspNetUser> CheckEmail(string email);
        Task<AspNetUser> checkEmailPassword(AspNetUser user);
        Task<User> GetUser(string email);
        Task<AspNetUser> GetAspNetUser(string email);
        Task<string> PatientForgotPassword(CreateAccountViewModel createAccountViewModel);
    }
}
