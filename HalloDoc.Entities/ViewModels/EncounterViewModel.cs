using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class EncounterViewModel
    {
        [Required(ErrorMessage = "Enter First Name Here")]
        public string FirstName { get; set; }=string.Empty;

        [Required(ErrorMessage = "Enter First Name Here")]
        public string LastName { get; set; } = string.Empty;
        public string? Location { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enter First Name Here")]
        public string PhoneNumber { get; set; } = string.Empty;
        public string? MedicalReport { get; set; } = string.Empty;
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Enter First Name Here")]
        public string Email { get; set; } = string.Empty;
        public string? HistoryOfPresentIllness { get; set; } = string.Empty;
        public string? MedicalHistory { get; set; } = string.Empty;
        public string? Medications { get; set; } = string.Empty;
        public int? Temp { get; set; }
        public int? BloodPressureSystolic { get; set; }
        public int? BloodPressureDiastolic { get; set; }
        public string? Heent { get; set; } = string.Empty;
        public string? Chest { get; set; } = string.Empty;   
        public int? Hr { get; set; }
        public string? Allergies { get; set; } = string.Empty;
        public string? Cv { get; set; } = string.Empty;
        public string? o2 { get; set; }

        public string? ABD { get; set; } = string.Empty;
        public int RequestID { get; set; }
        public string? Extr { get; set; } = string.Empty;
        public string? Skin { get; set; } = string.Empty;
        public string? Neuro { get; set; } = string.Empty;
        public string? Diagnosis { get; set; } = string.Empty;
        public string? MedicationsDispensed { get; set; } = string.Empty;
        public string? Followup { get; set; } = string.Empty;
        public string? Other { get; set; } = string.Empty;
        public string? TreatmentPlan { get; set; } = string.Empty;
        public string? Procedures { get; set; } = string.Empty;
        public int? Rr { get; set; }
        public int? Pain { get; set; }

        public bool? IsChanged { get; set; }
        public bool? IsFinalized { get; set; }
    }
}
