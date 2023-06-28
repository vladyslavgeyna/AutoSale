namespace AutoSale.Domain.Models
{
    public class CarComparison
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public User User { get; set; } = null!;

        public int CarAdId { get; set; }

        public CarAd CarAd { get; set; } = null!;
    }
}