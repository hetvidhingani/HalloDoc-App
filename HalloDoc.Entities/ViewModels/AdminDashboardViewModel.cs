using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int requestID { get; set; }
        public int Status { get; set; }
        public int RequestTypeID { get; set; }
        public int? RequstClientId { get; set; }
        public string PatientName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Requestor { get; set; }
        public DateTime RequestedDate { get; set; }
        public string PatientPhone { get; set; }
        public string? RequestorPhone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string? PhysicianName { get; set; }
        public DateTime? DateOfService { get; set; }
        public int RegionId { get; set; }
        public int NewCount { get; set; }
        public int PendingCount { get; set; }
        public int ActiveCount { get; set; }
        public int ConcludeCount { get; set; }
        public int UnpaidCount { get; set; }
        public int ToCloseCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Region> State { get; set; }

        public string Email { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public List<AdminDashboardViewModel> PagingData { get; set; }
        public string stateTab { get; set; }

    }
}
