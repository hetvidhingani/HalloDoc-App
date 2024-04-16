using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class AdminMyProfileViewModel
    {
        public string UserName { get; set; }=string.Empty;
        public int Status { get; set; }

        [Required( ErrorMessage = "Password must be at least 6 characters long.")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter First Name Here")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Last Name Here")]
        public string LastName { get; set; }

        public int AdminID { get; set; }

        [Required(ErrorMessage = "Enter your Email Here")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Invalid phone number format. Use country code followed by 10 digits.")]
        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        [Required(ErrorMessage = "Enter Address Here")]
        public string Address1 { get; set; }

        [Required(ErrorMessage = "Enter City Here")]
        public string City { get; set; }

        [Required(ErrorMessage = "Enter State Here")]
        public List<Region> State { get; set; }

        [Required(ErrorMessage ="Zip Code is Required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid PIN code.")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Enter Address Here")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Invalid phone number format. Use country code followed by 10 digits.")]
        public string BillingPhoneNumber { get; set; }

        public int RegionId { get; set; }

        [Required(ErrorMessage = "Enter your Email Here")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Compare("Email", ErrorMessage = "Email do not match.")]

        public string ConfirmEmail { get; set; }
        public List<int>? notification { get; set; }
        public List<Region>? AdminRegions { get; set; }
        public List<Role> roles { get; set; }
        public int? RoleID { get; set; }
        public List<int> AdminRegionID { get; set; }


    }
}
