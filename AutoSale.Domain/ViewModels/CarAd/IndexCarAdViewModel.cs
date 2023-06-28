using AutoSale.Domain.Enum;
using AutoSale.Domain.Enum.Car;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace AutoSale.Domain.ViewModels.CarAd
{
    public class IndexCarAdViewModel
    {
        public IPagedList<Models.CarAd> CarAds { get; set; } = null!;

        public int AllCarAdsCount { get; set; }

        public List<SelectListItem> CarBrands { get; set; } = new();
        
        public List<SelectListItem> CarModels { get; set; } = new();

        public int CarBrandId { get; set; }
        
        public int CarModelId { get; set; }
        
        public Region Region { get; set; }
        
        public int YearFrom { get; set; }
        
        public int YearTo { get; set; }
        
        public int PriceFrom { get; set; }
        
        public int PriceTo { get; set; }

        public CarAdsOrderByOptions CarAdsOrderByOptions { get; set; }
    }
}