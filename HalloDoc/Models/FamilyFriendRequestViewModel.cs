using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace HalloDoc.Models
{
    public class FamilyFriendRequestViewModel
    {
       
        [Required(ErrorMessage = "Enter Patient's First Name Here")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Enter Patient's Last Name Here")]

        public string? LastName { get; set; }
        [Required(ErrorMessage = "Enter Patient's Phone Number Here")]

        public string? PhoneNumber { get; set; }
        public IFormFile? File { get; set; }
        [Required(ErrorMessage = "Enter Patient's Email Here")]


        public string? Email { get; set; }
      
        public string? RelationName { get; set; }
        [Required]
        public string? Symptoms { get; set; }
        public string? RegionId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
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
