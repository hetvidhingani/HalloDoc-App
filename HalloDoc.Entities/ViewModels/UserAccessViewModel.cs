using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class UserAccessViewModel
    {
       public List<TableUserAccess>? PagingData { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int AccountTypeId { get; set; }

        public int TotalCount { get; set; }

        public int FirstItemIndex { get; set; }

        public int LastItemIndex { get; set; }

    }
    public class TableUserAccess
    {
        public int AdminID { get; set; }

        public int PhysicianID { get; set; }

        public int AccountTypeID { get; set; }

        public string AccountType { get; set; } = string.Empty;

        public string AccountPOC { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int OpenRequest { get; set; } 
    }
}
