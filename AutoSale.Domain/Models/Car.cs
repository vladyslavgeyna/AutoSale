using AutoSale.Domain.Enum.Car;

namespace AutoSale.Domain.Models
{
    public class Car
    {
        public int Id { get; set; }
        
        public int CarBrandId { get; set; }
        
        public CarBrand CarBrand { get; set; } = null!;

        public int CarModelId { get; set; }

        public CarModel CarModel { get; set; } = null!;

        public int YearOfProduction { get; set; }

        public double? EngineCapacity { get; set; }

        public Fuel Fuel { get; set; }

        public Color Color { get; set; }

        public Transmission Transmission { get; set; }

        public Region Region { get; set; }
        
        public decimal Price { get; set; }

        public int CurrencyId { get; set; }

        public Currency Currency { get; set; } = null!;

        public WheelDrive WheelDrive { get; set; }

        public int NumberOfSeats { get; set; }
        
        public int Mileage { get; set; }
        
        public string? AdditionalOptions { get; set; }
    }
}