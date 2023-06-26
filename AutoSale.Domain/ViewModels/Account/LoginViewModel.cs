using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Incorrect email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        
        public string? ReturnUrl { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        
        public List<string> ErrorMessages { get; set; } = new();
    }
}