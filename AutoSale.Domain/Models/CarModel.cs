﻿namespace AutoSale.Domain.Models
{
    public class CarModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int CarBrandId { get; set; }

        public CarBrand CarBrand { get; set; } = null!;
    }
}