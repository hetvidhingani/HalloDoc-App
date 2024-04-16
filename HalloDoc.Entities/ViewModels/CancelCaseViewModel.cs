using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class CancelCaseViewModel
    {
        [Required(ErrorMessage = "Please Select Reason!")]
        public int CaseTagID { get; set; }

        public List<CaseTag>? Case { get; set; }=null;

        //[Required(ErrorMessage = "Please Provide your Reason for cancellation !")]
        public required string AdditionalNotes { get; set; }

        public int RequestClientID { get; set; }

        public  string? PatientName { get; set; }
        
        
    }
}
