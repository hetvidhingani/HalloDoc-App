using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class TimeSheetViewModel:sheets
    {
        public DateOnly startDate {  get; set; }
        public DateOnly endDate { get; set; }
        public int totalHr { get; set; }
        public bool holiday {  get; set; }
        public int Housecalls { get; set; }
        public int phoneConsult { get; set; }
    }
    public class sheets
    {
        public List<TimeSheetViewModel>? TimeSheets { get; set; }
    }
    
}
