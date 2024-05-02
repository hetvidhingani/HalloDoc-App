using HalloDoc.Entities.DataModels;
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
        public int? payrate { get; set; }
        public string? category {  get; set; }
        public int? payrate1 { get; set; }
        public int? payrate2 { get; set; }
        public int? payrate3 { get; set; }
        public int? payrate4 { get; set; }
        public int? payrate5 { get; set; }
        public int? payrate6 { get; set; }
        public int? payrate7 { get; set; }
        public List<int?> payRates { get; set; }
        public int payrateId { get; set; }
        public int? categoryId { get; set; }
        public int? rate { get; set; }
        public int? cid { get; set; }
        public List<Physician> physicians { get; set; }
        public string status { get; set; }
    }
    public class sheets
    {
        public List<TimeSheetViewModel>? TimeSheets { get; set; }
    }

}
