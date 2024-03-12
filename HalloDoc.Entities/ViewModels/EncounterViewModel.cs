using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class EncounterViewModel
    {
        public int RequestID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string MedicalReport { get; set; }
        public DateTime Date { get; set; }
        public string Email { get; set; }
        public string HistoryOfPresentIllness { get; set; }
        public string MedicalHistory { get; set; }
        public string Medications { get; set; }
        public decimal Temp { get; set; }
        public int BloodPressureSystolic { get; set; }
        public int BloodPressureDiastolic { get; set; }
        public string Heent { get; set; }
        public string Chest { get; set; }
        public int Hr { get; set; }
        public string Allergies { get; set; }
        public string Cv { get; set; }
        public string o2 { get; set; }

        public string ABD { get; set; }
        public string Extr { get; set; }
        public string Skin { get; set; }
        public string Neuro { get; set; }
        public string Diagnosis { get; set; }
        public string MedicationsDispensed { get; set; }
        public string Followup { get; set; }
        public string Other { get; set; }
        public string TreatmentPlan { get; set; }
        public string Procedures { get; set; }
        public int Rr { get; set; }
        public int Pain { get; set; }

        public bool IsChanged { get; set; }
        public bool IsFinalized { get; set; }
    }
}
