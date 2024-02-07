using HalloDoc.DataModels;

namespace HalloDoc.Models
{
    public class PatientRequestViewModel
    {

        public List<UserViewModel> User { get; set; } = new List<UserViewModel>();
        public List<RequestViewModel> Request { get; set; } = new List<RequestViewModel>();


    }
    public class RequestViewModel
    {
        public int RequestId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? DocumentPath { get; set; }


        public string? Symptoms { get; set; }
    }
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string? Mobile { get; set; }
        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public DateOnly? DateOfBirth { get; set; }

    }
}
