using System.ComponentModel.DataAnnotations;
using HalloDoc.Entities.DataModels;
using Microsoft.AspNetCore.Http;


namespace HalloDoc.Entities.ViewModels
{
    public class OtherRequestViewModel
    {
        [Required(ErrorMessage = "Enter Patient's Symptoms Here")]
        public string Symptoms { get; set; }

        [Required(ErrorMessage = "Enter Patient's First Name Here")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Patient's Last Name Here")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Invalid phone number format. Use country code followed by 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter Patient's Email Here")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Select Your State.")]
        public int RegionId { get; set; }
        
        [Required(ErrorMessage = "Enter Street Here")]
        public string Street { get; set; }
    
        [Required(ErrorMessage = "Enter City Here")]
        public string City { get; set; }

        [Required(ErrorMessage = "Enter Zip Code Here.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid PIN code.")]
        public string? ZipCode { get; set; }

        [Required(ErrorMessage = "Enter First Name Here")]
        public string? ClientFirstName { get; set; }

        [Required(ErrorMessage = "Enter Last Name Here")]
        public string? ClientLastName { get; set; }

        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Invalid phone number format. Use country code followed by 10 digits.")]
        public string ClientPhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter your Email Here")]
        [EmailAddress]
        public string? CLientEmail { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        public string? ClientProperty { get; set; }
        public string? ClientCaseNo { get; set; }
        public List<Region>? State { get; set; }
        public string? ClientZipCode { get; set; }
        public string? ClientStreet { get; set; }
        public string? ClientCity { get; set; }
        public int? IntDate { get; set; }
        public int? IntYear { get; set; }
        public string? StrMonth { get; set; }
        public IFormFile? File { get; set; }
        public string? RelationName { get; set; }
        public int? ClientRegionId { get; set; }

    }
}
