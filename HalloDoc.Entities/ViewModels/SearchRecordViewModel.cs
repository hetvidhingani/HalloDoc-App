using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class SearchRecordViewModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int currData { get; set; }
        public int TotalCount { get; set; }
        public List<SearchRecordTable> PagingData { get; set; }
        public int FirstItemIndex { get; set; }
        public int LastItemIndex { get; set; }
        public List<Status> StatusData { get; set; } 
        public int statusID { get; set; }
        public int requestType { get; set; } 
        
    }
    public class SearchRecordTable
    {
        public int RequestClientId { get; set; }
        public string PatientName { get; set; }
        public string Requestor { get; set; }
        public DateTime DateOfService { get; set; }
        public DateTime CloseCaseDate { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string zip {  get; set; }
        public string requestStatus { get; set; }
        public string physician { get; set; }
        public string PhysicianNote { get; set; }
        public string CancelByProviderNote { get; set; }
        public string AdminNote { get; set;}
        public string PatientNote { get; set;}

    }
}
