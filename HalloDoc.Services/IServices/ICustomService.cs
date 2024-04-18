﻿using HalloDoc.Entities.DataModels;
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
        string SendEmail(string email, string link, string subject, string body,int id,int AdminId,int providerId,List<string> attachmentFilePath = null);
        Task<int> GetCount(int statusId);

        Task<DashboardViewModel> ViewDocument(int Id);
        Task<string> ViewDocument(IFormFile a, int Id, int adminID, int userid);
        Task<RequestWiseFile> DownloadFile(int fileId);
        Task<byte[]> DownloadAllByChecked(IEnumerable<int> documentValues);
        Task<byte[]> DownloadAll(IEnumerable<int> documentValues, int? requestid);
        Task<string> PatientForgotPassword(CreateAccountViewModel createAccountViewModel);
        Task<string> AdminResetPassword(CreateAccountViewModel createAccountViewModel);
       // void EmailLog(int requestclientID, string Email,string link,string  subject,string body,int AdminId);
        AspNetUser checkEmailPassword(string data, string password);
        AspNetUser checkIfExist(string email);
        AspNetUserRole checkIfRoleExist(string id);
        Physician GetPhysician(string email);
        Task<List<RequestWiseFile>> GetFilesSelectedByFileID(List<int> selectedFilesIds);
        Task<object> sendAgreement(int id);
        Task<object> AcceptAgreement(int id);
        Task<object> ConfirmCancelAgreement(int id, string note);
        Task<RequestClient> GetRequestClientByID(int id);
    }
}
