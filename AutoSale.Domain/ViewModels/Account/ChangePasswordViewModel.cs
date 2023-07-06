using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "The minimum password length must be at least 5 characters long")]
        [MaxLength(20, ErrorMessage = "The maximum password length is 20 characters long")]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "The minimum password length must be at least 5 characters long")]
        [MaxLength(20, ErrorMessage = "The maximum password length is 20 characters long")]
        [Display(Name = "New password")]
        public string NewPassword { get; set; } = null!;
        
        [Required(ErrorMessage = "Confirm new password is required")]
        [Display(Name = "Confirm new password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password don't match")]
        public string ConfirmNewPassword { get; set; } = null!;
        
        public List<string> ErrorMessages { get; set; } = new();
    }
}