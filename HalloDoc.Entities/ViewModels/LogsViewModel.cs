using HalloDoc.Entities.DataModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class LogsViewModel
    {
        public List<Role> role { get; set; }

        public int RoleId { get; set; }
   
        public List<TableModelLogs>? paging { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public int FirstItemIndex { get; set; }

        public int LastItemIndex { get; set; }

        public int type { get; set; }
    }
    public class TableModelLogs
    {
        public int EmailLogId { get; set; }

        public int SMSLogId { get; set; }

        public string reciverName { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } 

        public DateTime SentDate { get; set; }

        public BitArray? sent { get; set; }
        public int? sentTries { get; set; } 

        public string confirmationNumber { get; set; } = string.Empty;

        public string action { get; set; } = string.Empty;

        public string roleName { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;

        public string PhoneNumber {  get; set; } = string.Empty;
    

    }
}
