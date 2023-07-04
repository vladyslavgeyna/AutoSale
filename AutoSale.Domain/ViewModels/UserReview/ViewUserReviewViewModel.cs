using AutoSale.Domain.Models;

namespace AutoSale.Domain.ViewModels.UserReview
{
    public class ViewUserReviewViewModel
    {
        public List<Models.UserReview> UserReviews { get; set; } = new();

        public bool IsAuthenticated { get; set; }
        
        public string? CurrentUserId { get; set; }

        public User UserTo { get; set; } = null!;
    }
}