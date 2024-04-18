using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class ProviderDashboardViewModel
    {
        public string stateTab;

        public int NewCount { get; set; }
        public int PendingCount { get; set; }
        public int ActiveCount { get; set; }
        public int ConcludeCount { get; set; }
        public string PatientName { get; set; }
        public string? PatientPhone { get; set; }
        public string? RequestorPhone { get; set; }
        public string Address { get; set; }
        public int RequestTypeID { get; set; }
        public int RequstClientId { get; set; }
        public int requestID { get; set; }
        public string? StateofTable { get; set; }
        public List<ProviderDashboardViewModel> PagingData { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int FirstItemIndex { get; set; }
        public int LastItemIndex { get; set; }
        public int PhysicianId { get; set; }
        public string note { get; set; }
        public int requestClientId { get; set; }
    }
}
