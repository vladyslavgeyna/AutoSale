
using AutoSale.Domain.Models;

namespace AutoSale.Domain.ViewModels.CarAd
{
    public class ViewCarAdViewModel
    {
        public Models.CarAd CarAd { get; set; } = null!;

        public List<CarImage> Images { get; set; } = new();
        
        public string? Message { get; set; }

        public string? CurrentUserId { get; set; }

        public bool IsAuthenticated { get; set; }

        public string? UserImageName { get; set; }

        public int CountOfAddedToFavorite { get; set; }
    }
}