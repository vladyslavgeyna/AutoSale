using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.ViewModels.Account
{
    public class ExternalLoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Incorrect email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "The minimum name length must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "The maximum name length is 50 characters long")]
        public string Name { get; set; } = null!;
        
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last name")]
        [MinLength(2, ErrorMessage = "The minimum last name length must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "The maximum last name length is 50 characters long")]
        public string LastName { get; set; } = null!;
        
        [Required(ErrorMessage = "Surname is required")]
        [MinLength(2, ErrorMessage = "The minimum surname length must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "The maximum surname length is 50 characters long")]
        public string Surname { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone number")]
        [RegularExpression(@"^0[3569]\d{8}$", ErrorMessage = "Incorrect phone number")]
        public string PhoneNumber { get; set; } = null!;
        
        public string? ReturnUrl { get; set; }

        public string? ProviderDisplayName { get; set; } = null!;
    }
}