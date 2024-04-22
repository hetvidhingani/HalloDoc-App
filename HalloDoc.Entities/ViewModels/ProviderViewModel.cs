using HalloDoc.Entities.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class ProviderViewModel
    {
        public string? UserName { get; set; } = string.Empty;
        public int Status { get; set; }

        [Required(ErrorMessage = "Password must be at least 6 characters long.")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter First Name Here")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Last Name Here")]
        public string LastName { get; set; }

        public int PhysicianId { get; set; }

        [Required(ErrorMessage = "Enter your Email Here")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Invalid phone number format. Use country code followed by 10 digits.")]
        public string PhoneNumber { get; set; }

        public List<Role> Role { get; set; }
        public int RoleId { get; set; }
        public List<int>? notification { get; set; }


        [Required(ErrorMessage = "Enter Address Here")]
        public string Address1 { get; set; }

        [Required(ErrorMessage = "Enter City Here")]
        public string City { get; set; }

        [Required(ErrorMessage = "Enter State Here")]
        public List<Region>? State { get; set; }

        [Required(ErrorMessage = "Zip Code is Required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid PIN code.")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Enter Address Here")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Invalid phone number format. Use country code followed by 10 digits.")]
        public string BillingPhoneNumber { get; set; }
        [Required(ErrorMessage = "Enter State Here")]

        public int RegionId { get; set; }

        public List<Region>? Regions { get; set; }

        [Required(ErrorMessage = "Enter your Email Here")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ConfirmEmail { get; set; }

        public int? AdminRegionID { get; set; }
        public List<AdminRegion>? PhysicianRegions { get; set; }
        public string? MedicalLicense { get; set; }
        public string? NPINumber { get; set; }
        public string? SyncEmail { get; set; }
        public string? description { get; set; }
        [Required(ErrorMessage = "Enter Business Name Here")]

        public string? BusinessName { get; set;}
        [Required(ErrorMessage = "Enter Business Website Here")]

        public string? BusinessWebsite { get; set; }
        public IFormFile? File { get; set; }
        public IFormFile? Signature { get; set; }
        public string? AdminNotes { get; set; }
        public string Name { get; set; }
        public string onCallStatus { get; set; }
        [Required(ErrorMessage = "please select status.")]
        public int statusId { get; set; }
        public List<Status>? status { get; set; }
        public List<ProviderViewModel> Providers { get; set; }
        public string RegionName { get; set; }
        public string RoleName { get; set; }
        public List<Physician> physicians { get; set; }
        public bool IsAgreementDoc {  get; set; }
        public bool isbackgroundcheck { get; set; }
        public bool Ishippa { get; set; }
        public bool IsAgreementDocnondisclosure { get; set; }

        public IFormFile? Photo { get; set; }
        public IFormFile? contractoragreement { get; set; }
        public IFormFile? backgroundcheck { get; set; }
        public IFormFile? hippa { get; set; }
        public IFormFile? nondisclosure { get; set; }
        public List<RegionViewModel> StateCheckbox { get; set; }
        public List<PhysicianRegion> physicianRegion { get; set; }
        public List<int>? physicianRegionids { get; set; }
        public string? filename { get; set; }
        public string longitude {  get; set; }
        public string latitude { get; set; }

    }

}
