using HalloDoc.DataModels;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Models
{
    public class CreateAccountViewModel
    {
        [Required]
        //[EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string  ConfirmPassword { get; set; }
        public int RequestID { get; set; }

    }
}
