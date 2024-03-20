using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class AssignCaseViewModel
    {
        public string CaseTagID { get; set; }
        //public List<CaseTag> Case { get; set; }
        public string? AdditionalNotes { get; set; }
        public int RequestID { get; set; }
        public int RequestClientID { get; set; }

        [Required(ErrorMessage = "Please Select State!")]
        public int regionID { get; set; }

        [Required(ErrorMessage ="Please Select Physician!")]
        public int physicianID { get; set; }

        public List<Region> Region { get; set; }

        public List<Physician> Physician { get; set; }


    }
}
