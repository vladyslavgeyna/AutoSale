namespace AutoSale.Domain.ViewModels.CarAd
{
    public class MyAdsViewModel
    {
        public List<Models.CarAd> CarAds { get; set; } = new();

        public string? Message { get; set; }
    }
}