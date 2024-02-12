using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Models
{
    public class BusinessRequestViewModel
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]

        public string? Email { get; set; }
        [Required]
        public string? Symptoms { get; set; }
        public string? RegionId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        [Required]

        public string? ClientFirstName { get; set; }
        [Required]
        public string? ClientLastName { get; set; }
        [Required]

        public string? ClientPhoneNumber { get; set; }
        [Required]

        public string? CLientEmail { get; set; }
        public string? ClientProperty { get; set; }
        public string? ClientCaseNo { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
}
