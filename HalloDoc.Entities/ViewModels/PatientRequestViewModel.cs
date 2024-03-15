using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using HalloDoc.Entities.DataModels;
using Microsoft.AspNetCore.Http;


namespace HalloDoc.Entities.ViewModels
{
    public class PatientRequestViewModel
    {
        //[Required(ErrorMessage = "Password is required.")]
        //[StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one special character.")]
        
        public string? Password { get; set; }

        //[Required(ErrorMessage = "Confirm password is required.")]
        //[Compare("Password", ErrorMessage = "Passwords do not match.")]
       
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }

        //[RegularExpression(@"^[1-9]\d{5}$", ErrorMessage = "Invalid  zip code")]
        public string? ZipCode { get; set; }

       
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enter First Name Here")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Last Name Here")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter your Email Here")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Your Symptoms Here")]
        public string Symptoms { get; set; }


        public IFormFile? File { get; set; }
        public string? RelationName { get; set; }
        public int? RegionId { get; set; }

        public string? City { get; set; }
        public string? Street { get; set; }
        public List<Region> State { get; set; }
        public PatientRequestViewModel()
        {
            State = new List<Region>();
        }
        public int? IntDate { get; set; }
        public int? IntYear { get; set; }
        public string? StrMonth { get; set; }

    }

    
}

