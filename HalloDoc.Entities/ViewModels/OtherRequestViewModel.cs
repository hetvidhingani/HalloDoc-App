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

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter Patient's Email Here")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public IFormFile? File { get; set; }
        public string? RelationName { get; set; }

        public int? RegionId { get; set; }
        public int? ClientRegionId { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }
        public List<Region> State { get; set; }
        public OtherRequestViewModel()
        {
            State = new List<Region>();
           
        }

        [RegularExpression(@"^[1-9]\d{5}$", ErrorMessage = "Invalid  zip code")]
        public string? ZipCode { get; set; }

        [Required(ErrorMessage = "Enter First Name Here")]
        public string? ClientFirstName { get; set; }

        [Required(ErrorMessage = "Enter Last Name Here")]
        public string? ClientLastName { get; set; }

        public string? ClientPhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter your Email Here")]
        [EmailAddress]
        public string? CLientEmail { get; set; }

     
        public DateOnly? DateOfBirth { get; set; }
        public string? ClientProperty { get; set; }
        public string? ClientCaseNo { get; set; }

    //    [RegularExpression(@"^[1-9]\d{5}$", ErrorMessage = "Invalid  zip code")]
        public string? ClientZipCode { get; set; }
        public string? ClientStreet { get; set; }
        public string? ClientCity { get; set; }
        
        
    }
}
