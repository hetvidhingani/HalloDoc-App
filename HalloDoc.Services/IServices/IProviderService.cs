using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.IServices
{
    public interface IProviderService
    {
        List<Region> getstateDropdown();

        Task<int> GetCount(int statusId, int physicianID);

        List<ProviderDashboardViewModel> ProviderTable(int providerId, string state, List<ProviderDashboardViewModel> list, int status);

        ProviderDashboardViewModel Pagination(string state, int CurrentPage, string? PatientName, int? ReqType, List<ProviderDashboardViewModel> newState);

        Task<object> ViewCase(int userId);

        Task<string> EditNewRequest(ViewCaseViewModel viewModel, int? userId);

        Task<ViewNotesViewModel> ViewNotes(int id);

        Task<object> AddNotes(string additionalNotes, int id, string AdminID);

        Task AcceptCase(int requestId, int providerId);

        Task TransferCase(int id,int providerId,string note);
        bool IsDeleted(BitArray? isDeleted);
        Task<object> DeleteFile(int fileID, int? reqID);
    }
}
