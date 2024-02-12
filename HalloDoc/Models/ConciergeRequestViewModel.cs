using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Models
{
    public class ConciergeRequestViewModel
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
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

        public string? ClientPhoneNumber { get; set; }
        [Required]

        public string? CLientEmail { get; set; }
        public string? ClientProperty { get; set; }
        [Required]

        public string? ClientState { get; set; }
        [Required]

        public string? ClientCity { get; set; }
        [Required]

        public string? ClientStreet { get; set; }
        [Required]

        public string? ClientZipCode { get; set; }

        public DateOnly? DateOfBirth { get; set; }
    }
}