using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class ProviderInfoViewModel
    {

        public int? RegionId { get; set; }

        public List<Region>? Regions { get; set; }

        public List<TableProviderInfo>? PagingData { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int AccountTypeId { get; set; }

        public int TotalCount { get; set; }

        public int FirstItemIndex { get; set; }

        public int LastItemIndex { get; set; }
    }
    public class TableProviderInfo
    {
        public string stopNotification { get; set; }=string.Empty;

        public string ProviderName { get; set; }= string.Empty;

        public string roleName { get; set; } = string.Empty;

        public string onCallStatus { get; set; } = string.Empty;

        public string status { get; set; } = string.Empty;

        public int PhysicianID { get; set; }

    }
}
