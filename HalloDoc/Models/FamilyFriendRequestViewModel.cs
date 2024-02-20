using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace HalloDoc.Models
{
    public class FamilyFriendRequestViewModel
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
        
        public string? RegionId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        [Required(ErrorMessage = "Zip is Required")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        public string? ZipCode { get; set; }

        [Required(ErrorMessage = "Enter First Name Here")]
        public string? ClientFirstName { get; set; }

        [Required(ErrorMessage = "Enter Last Name Here")]
        public string? ClientLastName { get; set; }

        public string? ClientPhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter your Email Here")]
        public string? CLientEmail { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? ClientProperty { get; set; }
        public string? ClientCaseNo { get; set; }
        public string? ClientZipCode { get; set; }
        public string? ClientState { get; set; }
        public string? ClientCity { get; set; }
        public string? ClientStreet { get; set; }

    }
}
