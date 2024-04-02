using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class VendorsViewModel
    {
        public List<HealthProfessional> vendor {  get; set; }
        public List<HealthProfessionalType> ProfessionType { get; set; }
        public int VendorID { get; set; }

        [Required(ErrorMessage = "Please Select Profession !")]
        public int ProfessionTypeID { get; set;}

        public List<Region> regions { get; set; }
        [Required(ErrorMessage = "Please Select State !")]

        public int RegionID { get; set; }
        public int Profession {  get; set; }
        public string vendorName { get; set; }
        [Required(ErrorMessage = "Please Provide Phone Number !")]
        public string BusinessContact { get; set; }

        [Required(ErrorMessage = "Enter your Email Here")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public string? FaxNumber { get; set; }
        [Required(ErrorMessage = "Please Provide Street !")]

        public string street { get; set; }
        [Required(ErrorMessage = "Please Provide City!")]

        public string City { get; set; }

        [Required(ErrorMessage = "Enter Zip Code Here.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid PIN code.")]
        public string zip {  get; set; }
        public DateTime CreatedDate { get; set; }


        [Required(ErrorMessage = "Enter Phone Number Here")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Invalid phone number format. Use country code followed by 10 digits.")]
        public string PhoneNumber { get; set; }


    }
}
