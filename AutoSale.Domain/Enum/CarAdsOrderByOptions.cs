using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.Enum
{
    public enum CarAdsOrderByOptions
    {
        [Display(Name = "By default")]
        ByDefault = 1,
        [Display(Name = "From cheap to expensive")]
        FromCheapToExpensive,
        [Display(Name = "From expensive to cheap")]
        FromExpensiveToCheap,
        [Display(Name = "Year of production, ascending")]
        YearOfProductionInAscendingOrder,
        [Display(Name = "Year of production, descending")]
        YearOfProductionDescending,
        [Display(Name = "Mileage, ascending")]
        MileageInAscendingOrder,
        [Display(Name = "Mileage, descending")]
        MileageDescending,
    }
}