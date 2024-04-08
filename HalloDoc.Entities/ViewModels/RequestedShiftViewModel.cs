using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class RequestedShiftViewModel
    {
        public List<shiftTable> PagingData { get; set; }
        public List<Region> regions { get; set; }
        public int RegionId { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public int TotalCount { get; set; }
        public int FirstItemIndex { get; set; }
        public int LastItemIndex { get; set; }
    }
    public class shiftTable
    {
        public string staff {  get; set; }
        public string day { get; set; }
        public string time { get; set; }
        public string region { get; set; }
        public int shiftdetailId { get; set; }
    }
}
