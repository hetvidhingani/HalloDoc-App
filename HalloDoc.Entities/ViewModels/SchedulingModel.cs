using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class SchedulingModel:CreateShiftViewModel
    {
        public List<Resources> resources { get; set; }
        public List<Events> events { get; set; }
        public int RegionIds{  get; set; }
        //public List<Region> regions { get; set; }
        public CreateShiftViewModel createShiftViewModel { get; set; }
    }
    public class Resources
    {
        public string id { get; set; }
        public string title { get; set; }
        public string avtar { get; set; }
    }
    public class Events
    {
        public string id { get; set; }
        public string resourceId { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string title { get; set; }
        public string color { get; set; }

    }
    public class CreateShiftViewModel
    {

        [Required(ErrorMessage = "Please Select Region. ")]
        public int regionId { get; set; }


        [Required(ErrorMessage = "Please Select Physician. ")]
        public int physicianID { get; set; }

        [Required(ErrorMessage = "Please Select date. ")]
        public DateOnly ShiftDate { get; set; }

        [Required(ErrorMessage = "Please Select start time. ")]
        public TimeOnly startTime { get; set; }

        [Required(ErrorMessage = "Please Select end time. ")]
        public TimeOnly endTime { get; set; }

        public bool toggle { get; set; } = false;
        public List<Physician>? physician { get; set; }

        public List<Region>? regions { get; set; }
        public int ShiftDetailId { get; set; }
        public int? repeatEnd { get; set; } = 0;
        public int provider {  get; set; }
    }
}
