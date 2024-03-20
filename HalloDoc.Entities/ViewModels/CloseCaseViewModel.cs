using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class CloseCaseViewModel
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enter your Email Here")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Invalid phone number format. Use country code followed by 10 digits.")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? ConfirmationNumber { get; set; } = string.Empty;
        public string? RequestID { get; set; } = string.Empty;
        public string? FileName { get; set; } = string.Empty;
        public string? UploadDate { get; set; } = string.Empty;
        public int RequstId { get; set; }
        public string? CreatedDate { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int RequestClientID { get; set; }

        public int RequestWiseFileID { get; set; }
        public bool isdeleted { get; set; }
        public List<RequestWiseFile>? RequestWiseFiles { get; set; }

    }
}
