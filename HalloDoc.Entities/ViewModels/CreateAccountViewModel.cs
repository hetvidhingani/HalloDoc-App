using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Entities.ViewModels
{
    public class CreateAccountViewModel
    {
        [Required]
        //[EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        //public int RequestID { get; set; }
        public object? Token { get; internal set; } = null;

    }
}
