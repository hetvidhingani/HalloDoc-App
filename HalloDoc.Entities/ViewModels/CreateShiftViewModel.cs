using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class CreateShiftViewModel
    {
        public List<Region> regions {  get; set; }
        public int regionId { get; set; }
        public List<Physician> physician { get; set; }
        public int physicianId { get; set; }
        public DateTime? ShiftDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public bool toggle {  get; set; }
        public int dayOfWeek { get; set; }
        public int repeatEnd { get; set; }
    }
}
