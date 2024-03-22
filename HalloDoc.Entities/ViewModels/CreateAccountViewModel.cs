using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Entities.ViewModels
{
    public class CreateAccountViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }


        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one special character.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("PasswordHash", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
     
        public object? Token { get; internal set; } = null;

    }
}
