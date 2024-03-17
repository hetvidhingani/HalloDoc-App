using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using HalloDoc.Entities.DataModels;
using Microsoft.AspNetCore.Http;


namespace HalloDoc.Entities.ViewModels
{
    public class PatientRequestViewModel
    {
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one special character.")]
        public string? Password { get; set; }


        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Invalid phone number format. Use country code followed by 10 digits.")]
        public string PhoneNumber { get; set; }


        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid PIN code.")]
        public string? ZipCode { get; set; }


        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }


        [Required(ErrorMessage = "Enter First Name Here")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Enter Last Name Here")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Enter your Email Here")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Enter Your Symptoms Here")]
        public string Symptoms { get; set; }

       
        [Required(ErrorMessage = "Enter City Here")]
        public string? City { get; set; }


        [Required(ErrorMessage = "Enter Street Here")]
        public string? Street { get; set; }


        [Required(ErrorMessage = "Enter Street Here")]
        public List<Region>? State { get; set; }

        public int? IntDate { get; set; }
        public int? IntYear { get; set; }
        public string? StrMonth { get; set; }
        public IFormFile? File { get; set; }
        public string? RelationName { get; set; }
        public int? RegionId { get; set; }

    }


}

