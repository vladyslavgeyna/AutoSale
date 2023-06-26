namespace AutoSale.Domain.Models
{
    public class CarImage
    {
        public int Id { get; set; }
        
        public int ImageId { get; set; }

        public Image Image { get; set; } = null!;

        public int CarId { get; set; }

        public Car Car { get; set; } = null!;

        public bool IsMain { get; set; }
    }
}