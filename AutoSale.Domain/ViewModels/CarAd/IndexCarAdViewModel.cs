using X.PagedList;

namespace AutoSale.Domain.ViewModels.CarAd
{
    public class IndexCarAdViewModel
    {
        public IPagedList<Models.CarAd> CarAds { get; set; } = null!;

        public int AllCarAdsCount { get; set; }
    }
}