using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string Password { get; set; } = null!;
        
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password don't match")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;
    }
}