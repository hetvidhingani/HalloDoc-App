using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class CancelCaseViewModel
    {
        public string CaseTagID { get; set; }
        public List<CaseTag> Case { get; set; }
        public string AdditionalNotes { get; set; }
        public int RequestID { get; set; }
        public int RequestClientID { get; set; }
        public string PatientName { get; set; }
        public int result {  get; set; }
        
    }
}
