namespace AutoSale.Domain.ViewModels.FavoriteAd
{
    public class IndexFavoriteAdViewModel
    {
        public List<Models.FavoriteAd> FavoriteAds { get; set; } = new();

        public string CurrentUserId { get; set; } = null!;
    }
}