using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class SchedulingModel
    {
        public List<Resources> resources { get; set; }
        public List<Events> events { get; set; }
        public int RegionId {  get; set; }
        public List<Region> regions { get; set; }
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
        public List<Region>? regions { get; set; }

        [Required(ErrorMessage = "Please Select Region. ")]
        public int regionId { get; set; }

        public List<Physician>? physician { get; set; }

        [Required(ErrorMessage = "Please Select Physician. ")]
        public int physicianId { get; set; }

        public DateOnly ShiftDate { get; set; }

        public TimeOnly startTime { get; set; }

        public TimeOnly endTime { get; set; }

        public bool toggle { get; set; } = false;

        public int ShiftDetailId { get; set; }
        public int repeatEnd { get; set; } = 0;
    }
}
