using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class TimeSheetViewModel : sheets
    {
        public DateOnly startDate { get; set; }
        public DateOnly endDate { get; set; }
        public int? totalHr { get; set; }
        public bool holiday { get; set; }
        public int? Housecalls { get; set; }
        public int? phoneConsult { get; set; }
        public int? shiftCount { get; set; }
        public int? NightShiftWeekend { get; set; }
        public int? HousecallNightsWeekend { get; set; }
        public int? PhoneconsultNightWeekend { get; set; }
        public string range { get; set; }
        public int? amount { get; set; }
        public string? item { get; set; }
        public IFormFile? bill { get; set; }
        public int physicianID { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int FirstItemIndex { get; set; }
        public int LastItemIndex { get; set; }
        public string? billname { get; set; }
    }
    public class sheets
    {
        public List<TimeSheetViewModel>? TimeSheets { get; set; }
    }

}
