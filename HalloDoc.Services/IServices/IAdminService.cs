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
        string SendEmail(string email, string link,string subject, string body, List<string> attachmentFilePath = null);
        Task<List<RequestWiseFile>> GetFilesSelectedByFileID(List<int> selectedFilesIds);
        Task<AspNetUser> checkEmailPassword(string email, string password);
        Task<User> GetUser(string email);
        Task<RequestClient> GetRequestClientByID(int id);
        Task<T> GetTempData<T>(string key);
        Task<object> AdminMyProfile(int? adminId);
        Task<int> GetCount(int statusId);
        Task<object> ViewCase(int userId);
        Task<int> GetUserByRequestClientID(int id);
        Task<Admin> GetAdmin(string email);
        Task<string> EditNewRequest(ViewCaseViewModel viewModel, int? userId);
        Task<ViewNotesViewModel> ViewNotes(int id);
        Task<object> AddNotes(string? additionalNotes, string? adminNotes, int id);
        Task<object> CancelCase(CancelCaseViewModel viewModel, int id);
        Task<string> ConfirmCancelCase(CancelCaseViewModel viewModel, int id);
        Task<object> AssignCase(AssignCaseViewModel viewModel, int id);
        Task<string> AssignRequest(AssignCaseViewModel viewModel, int id);
        Task<object> BlockCase(CancelCaseViewModel viewModel, int id);
        Task<string> BlockCaseRequest(CancelCaseViewModel viewModel, int id);
        Task<List<Physician>> GetPhysiciansByRegion(int regionId);
        Task<object> DeleteFile(int name,int? reqID);
        Task<object> SendOrder( int Id);
        Task<List<HealthProfessional>> GetBusinessByProfession(int professionId);
        Task<HealthProfessional> GetBusinessDetails(object BusinessId);
        Task<string> SendOrderDetails(SendOrderViewModel viewModel, int id , string adminID);
        Task<object> TransferCase(AssignCaseViewModel viewModel, int id);
        Task<string> TransferRequest(AssignCaseViewModel viewModel, int id);
        Task<string> ClearRequest(int? id);
        Task<object> sendAgreement( int id);
        Task<object> DashboardRegions(AdminDashboardViewModel viewModel);
        Task<object> AcceptAgreement(int id);
        Task<object> ConfirmCancelAgreement(int id, string note);
        List<AdminDashboardViewModel> Admintbl(string state,List<AdminDashboardViewModel> list, int status);
        Task<CloseCaseViewModel> CloseCase(int Id);
        Task<object> EditClose(CloseCaseViewModel viewModel, int Id);
        Task<string> ConfirmCloseCase(int id);
        Task<object> ResetPasswordAdmin(AdminMyProfileViewModel model);
        Task<object> SaveAdminInfo(AdminMyProfileViewModel model,List<int> ids);
        Task<object> SaveBillingInfo(AdminMyProfileViewModel model);
        Task<object> EncounterForm(int RequestId);
        Task<object> EncounterFormSaveChanges(EncounterViewModel model);
        AdminDashboardViewModel Pagination(string state,int CurrentPage, string? PatientName, int? ReqType, int? RegionId, List<AdminDashboardViewModel> newState);
        Task<object> CreateProvider(ProviderViewModel model);
        Task<object> Createprovider();
        Task<object> CreateAdmin();
        Task<object> CreateAdmin(AdminMyProfileViewModel model);
        ProviderInfoViewModel ProviderInformation(int RegionId,int CurrentPage);
        object RegionList();
        Task<object> EditProvider(int physicianID);
        Physician ContectProvider(int id);
        object ContectProviderModel(int id);
        Physician savePhysicianInformation(ProviderViewModel model,int id);
        object saveBillingInformation(ProviderViewModel model, int id);
        Task<object> resetPasswordProvider(int id, string password);
        Task<object> resetRoleStatus(ProviderViewModel model, int id);
        object providerProfile(ProviderViewModel model, int id);
        Task<object> AccountAccessTable(int CurrentPage);
        AccountAccessViewModel MenuName(int accountTypeId, int typename, int id = 0);
        AccountAccessViewModel CreateRole(int AdminId);
        Task<object> CreateAccess(AccountAccessViewModel viewModel);
        AccountAccessViewModel EditAccountAccess(int id, int AdminId);
        Task<object> submitEditAccess(AccountAccessViewModel viewModel);
        Task<object> deleteAccountAccess(int id);
        void DeleteProvider(int id);
        void StopNotificationPhysician(List<int> ids);
        UserAccessViewModel UserAccess(int accountTypeId, int CurrentPage);
        VendorsViewModel VendorDetail();
        VendorsViewModel VendorTable(int VendorProfessionTypeId,string VendorName,int CurrPage);
        VendorsViewModel AddVendor();
        Task<VendorsViewModel> AddVendor(VendorsViewModel model);
        VendorsViewModel EditVendor(int id);
        LogsViewModel Logs();
        LogsViewModel LogTable(int RoleID, string ReciverName, string email,string phoneNo, DateTime? createdDate, DateTime? sentDate,int type,int CurrPage);
        PatientHistoryViewModel PatientHistory(string FirstName, string LastName, string Email, string PhoneNumber,int CurrentPage);
        PatientRecordViewModel PatientRecordTable(int userID,int CurrentPage);

        BlockHistoryViewModel BlockHistoryTable(string FirstName, DateTime? Date, string Email, string PhoneNumber, int CurrPage);
        void UnblockRequest(int id);
        SearchRecordViewModel SearchRecordTable(int statusOfRequest, string Name, int requestType, DateTime? DateOfService, DateTime? ToDateOfService, string physician, string Email, string PhoneNumber, int CurrPage = 1);
        void DeleteRequest(int id);


    }
}
