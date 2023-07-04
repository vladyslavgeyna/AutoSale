using System.ComponentModel.DataAnnotations;
using AutoSale.Domain.Models;

namespace AutoSale.Domain.ViewModels.UserReview
{
    public class CreateUserReviewViewModel
    {
        public User? UserTo { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Review title")]
        [MinLength(5, ErrorMessage = "The title is too short")]
        [MaxLength(50, ErrorMessage = "The title is too long")]
        public string Title { get; set; } = null!;
        
        [Required(ErrorMessage = "Text is required")]
        [Display(Name = "Review text")]
        [MinLength(10, ErrorMessage = "The text is too short")]
        [MaxLength(250, ErrorMessage = "The text is too long")]
        public string Text { get; set; } = null!;

        [Required]
        public string UserIdTo { get; set; } = null!;
    }
}