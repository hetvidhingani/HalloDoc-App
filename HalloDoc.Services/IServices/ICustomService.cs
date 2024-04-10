using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.IServices
{
    public interface ICustomService
    {
        string SendEmail(string email, string link, string subject, string body,List<string> attachmentFilePath = null);
       
        Task<DashboardViewModel> ViewDocument(int Id);
        Task<string> ViewDocument(IFormFile a, int Id, int adminID, int userid);
        Task<RequestWiseFile> DownloadFile(int fileId);
        Task<byte[]> DownloadAllByChecked(IEnumerable<int> documentValues);
        Task<byte[]> DownloadAll(IEnumerable<int> documentValues, int? requestid);
        Task<string> PatientForgotPassword(CreateAccountViewModel createAccountViewModel);
        Task<string> AdminResetPassword(CreateAccountViewModel createAccountViewModel);
        void EmailLog(int requestclientID, string Email,string link,string  subject,string body,int AdminId);
        AspNetUser checkEmailPassword(string data, string password);
        AspNetUser checkIfExist(string email);
    }
}
