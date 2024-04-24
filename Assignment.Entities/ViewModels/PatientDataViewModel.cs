using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Entities.ViewModels
{
    public class PatientDataViewModel
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]

        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Age is required.")]

        public int Age { get; set; }

        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]

        public string PhoneNumber { get; set; }

        public int Gender { get; set; }

        [Required(ErrorMessage = "Please Select Disease.")]

        public string Disease { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]

        public string Doctor { get; set; }

        public int DoctorId { get; set; }
    }
}
