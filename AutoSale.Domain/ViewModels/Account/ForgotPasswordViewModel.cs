using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}