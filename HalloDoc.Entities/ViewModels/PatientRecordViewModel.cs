using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class PatientRecordViewModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int currData { get; set; }
        public int TotalCount { get; set; }
        public List<RecordTableModel> PagingData { get; set; }
        public int FirstItemIndex { get; set; }
        public int LastItemIndex { get; set; }
        public int userID { get; set; }

    }
    public class RecordTableModel
    {
        public int RequestId { get; set; }
        public int requestclientID { get; set; }
        public string confirmationNumber { get; set; }
        public string ProviderName { get; set; }
        public string status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ConcludedDate { get; set; }
        public string clientName { get; set; }

        public int userID { get; set; }

    }
}
