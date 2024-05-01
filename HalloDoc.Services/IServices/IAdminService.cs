using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.IServices
{

    public interface IAdminService
    {
        
        Task<T> GetTempData<T>(string key);
        Task<object> AdminMyProfile(int? adminId);
        Task<object> ViewCase(int userId);
        Task<Admin> GetAdmin(string email);
        Task<string> EditNewRequest(ViewCaseViewModel viewModel, int? userId);
        Task<ViewNotesViewModel> ViewNotes(int id);
        Task<object> AddNotes(string additionalNotes, int id,string AdminID);
        Task<object> CancelCase(CancelCaseViewModel viewModel, int id);
        Task<string> ConfirmCancelCase(CancelCaseViewModel viewModel, int id,int adminid);
        Task<object> AssignCase(AssignCaseViewModel viewModel, int id);
        Task<string> AssignRequest(AssignCaseViewModel viewModel, int id ,int adminid);
        Task<object> BlockCase(CancelCaseViewModel viewModel, int id);
        Task<string> BlockCaseRequest(CancelCaseViewModel viewModel, int id,int adminid);
        Task<List<Physician>> GetPhysiciansByRegion(int regionId);
        Task<object> DeleteFile(int name,int? reqID);
      
        Task<object> TransferCase(AssignCaseViewModel viewModel, int id);
        Task<string> TransferRequest(AssignCaseViewModel viewModel, int id,int adminID);
        Task<string> ClearRequest(int? id);
       
        Task<CloseCaseViewModel> CloseCase(int Id);
        Task<object> EditClose(CloseCaseViewModel viewModel, int Id);
        Task<string> ConfirmCloseCase(int id,int adminID);
        Task<object> ResetPasswordAdmin(AdminMyProfileViewModel model);
        Task<object> SaveAdminInfo(AdminMyProfileViewModel model,List<int> ids,int adminId);
        Task<object> SaveBillingInfo(AdminMyProfileViewModel model,int adminId);
        Task<object> EncounterForm(int RequestId);
        Task<object> EncounterFormSaveChanges(EncounterViewModel model);
        void sendSMS(ContactProviderViewModel model, int adminId);
        void SendSMS(string toPhoneNumber, string message);
        Task<object> CreateProvider(ProviderViewModel model);
        Task<object> Createprovider();
        Task<object> CreateAdmin();
        Task<object> CreateAdmin(AdminMyProfileViewModel model);
        ProviderInfoViewModel ProviderInformation(int RegionId,int CurrentPage);
        object RegionList();
        Task<object> EditProvider(int physicianID);
        Physician ContectProvider(int id);
        object ContectProviderModel(int id);
        Physician savePhysicianInformation(ProviderViewModel model,int id, List<int> ids);
        object saveBillingInformation(ProviderViewModel model, int id);
        Task<object> resetPasswordProvider(int id, string password);
        Task<object> resetRoleStatus(ProviderViewModel model, int id);
        object providerProfile(ProviderViewModel model, int id);
        object documentsProvider(ProviderViewModel model);
        Task<object> AccountAccessTable(int CurrentPage);
        AccountAccessViewModel MenuName(int accountTypeId, int typename, int id = 0);
        AccountAccessViewModel CreateRole(int AdminId);
        Task<object> CreateAccess(AccountAccessViewModel viewModel,string aspAdmin);
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
        void DeleteVendor(int id);
        LogsViewModel Logs();
        LogsViewModel LogTable(int RoleID, string ReciverName, string email,string phoneNo, DateTime? createdDate, DateTime? sentDate,int type,int CurrPage);
        PatientHistoryViewModel PatientHistory(string FirstName, string LastName, string Email, string PhoneNumber,int CurrentPage);
        PatientRecordViewModel PatientRecordTable(int userID,int CurrentPage);
        BlockHistoryViewModel BlockHistoryTable(string FirstName, DateTime? Date, string Email, string PhoneNumber, int CurrPage);
        void UnblockRequest(int id);
        SearchRecordViewModel SearchRecordTable(int statusOfRequest, string Name, int requestType, DateTime? DateOfService, DateTime? ToDateOfService, string physician, string Email, string PhoneNumber, int CurrPage = 1);
        void DeleteRequest(int id);
        List<Region> getstateDropdown();
        Task CreateRequestByAdmin(PatientRequestViewModel viewModel, int adminId);
        SchedulingModel Scheduling(int regionId);
        void AddShift(CreateShiftViewModel model, List<DayOfWeek> WeekDays,string adminId);
        CreateShiftViewModel GetShiftDetailsById(int shiftDetailsId);
        void EditShiftData(CreateShiftViewModel shiftData);
        object returnShift(int id);
        void deleteShift(int id);
        void deleteshifts(List<int> selectedShift);
        RequestedShiftViewModel RequestedShift(int month,int region,int CurrentPage = 1);
       void ApproveShift(List<int> selectedShift);
        List<PhysicianLocation> ProviderLocation();
        ProvidersOnCallViewModel? GetProvidersOnCall(int regionId);

        List<AdminDashboardViewModel> ExportAllData();
        List<AdminDashboardViewModel> Admintbl(string state, List<AdminDashboardViewModel> list, int status);
        AdminDashboardViewModel Pagination(string state, int CurrentPage, string? PatientName, int? ReqType, int? RegionId, List<AdminDashboardViewModel> newState);
        string UnscheduledPhysicians (string message);
        void addSignature(ProviderViewModel model, int id);
        public void EditPayrate(int physician, int value, int categoty);
        public TimeSheetViewModel payrate(int id);


    }
}
