using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class AssignCaseViewModel
    {
        public string CaseTagID { get; set; }
        public List<CaseTag> Case { get; set; }
        public string AdditionalNotes { get; set; }
        public int RequestID { get; set; }
        public int RequestClientID { get; set; }
        public int physicianID { get; set; }
        public int regionID { get; set; }
        public List<Region> Region { get; set; }
        public List<Physician> Physician { get; set; }


    }
}
