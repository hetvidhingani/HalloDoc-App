using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class PatientHistoryViewModel
    {
 
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int currData { get; set; }
        public int TotalCount { get; set; }
        public List<TableModel> PagingData { get; set; }
        public int FirstItemIndex { get; set; }
        public int LastItemIndex { get; set; }
    }
    public class TableModel
    {
        public int RequestClientId { get; set; }
        public int RequestId { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int? userID { get; set; }
    }
}
