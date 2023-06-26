namespace AutoSale.Domain.Models
{
    public class CarAd
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        public DateTime DateOfCreation { get; set; }

        public bool IsActive { get; set; }

        public string UserId { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}