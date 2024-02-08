namespace HalloDoc.Models
{
    public class FamilyFriendRequestViewModel
    {
        public int RequestId { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? DocumentPath { get; set; }
        public string? RelationName { get; set; }

        public string? Symptoms { get; set; }
        public string? RegionId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }

        public string FamilyFirstName { get; set; } = null!;
        public string? FamilyLastName { get; set; }
        public string? FamilyPhoneNumber { get; set; }
        public string? FamilyEmail { get; set; }
        
    }
}