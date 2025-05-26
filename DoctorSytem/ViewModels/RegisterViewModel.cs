using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
        public string? Role { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
