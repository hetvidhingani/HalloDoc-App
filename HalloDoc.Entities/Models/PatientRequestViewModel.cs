using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HalloDoc.Entities.Models
{
    public class PatientRequestViewModel 
    {
       
        [Required(ErrorMessage = "Enter First Name Here")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Enter Last Name Here")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",ErrorMessage = "Entered phone format is not valid.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter your Email Here")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Enter Your Symptoms Here")]
        public string? Symptoms { get; set; }

        [Required(ErrorMessage = "Enter Your Date of Birth Here")]
        public DateOnly? DateOfBirth { get; set; }
        public string? ZipCode { get; set; }

        public IFormFile? File { get; set; }
        public string? RelationName { get; set; }
        public string? RegionId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
       
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set;}

    }
}

