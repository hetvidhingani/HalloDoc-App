using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class ViewCaseViewModel
    {
        public string FirstName { get; set; }=string.Empty;
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Invalid phone number format. Use country code followed by 10 digits.")]
        public string PhoneNumber { get; set; }= string.Empty;

        [Required(ErrorMessage = "Enter your Email Here")]
        [EmailAddress(ErrorMessage = "Invalid email address")]

        public string Email { get; set; } = string.Empty;
        public string? Symptoms { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? RegionId { get; set; }
        public string? State { get; set; }
        
        public int requestclientID { get; set; }
        public int status { get; set; }


    }
}
