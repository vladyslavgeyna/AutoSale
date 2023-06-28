namespace AutoSale.Domain.ViewModels.CarComparison
{
    public class IndexCarComparisonViewModel
    {
        public List<Models.CarComparison> CarComparisons { get; set; } = new();

        public string CurrentUserId { get; set; } = null!;
    }
}