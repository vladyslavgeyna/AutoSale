using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.ViewModels.Account
{
    public class DeleteViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Enter your password")]
        public string Password { get; set; } = null!;
    }
}