using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class PatientDashboardViewModel
    {
        public List<DashboardData> DashboardData { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int currData { get; set; }
        public int TotalCount { get; set; }
        public int FirstItemIndex { get; set; }
        public int LastItemIndex { get; set; }
    }
    public class DashboardData
    {
        public int RequstId { get; set; }
        public string? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string? FileName { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public int? FileCount { get; set; }
        public string FirstName { get; set; }
        public int RequestWiseFileID { get; set; }
        public bool isdeleted { get; set; }
        public List<RequestWiseFile> RequestWiseFiles { get; set; }
        public string Email { get; set; }
    }
}
