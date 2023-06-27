using System.ComponentModel.DataAnnotations;
using AutoSale.Domain.Attributes;
using AutoSale.Domain.Enum.Car;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoSale.Domain.ViewModels.CarAd
{
    public class CreateCarAdViewModel
    {
        [Required(ErrorMessage = "Images is required")]
        [Display(Name = "Images. Maximum images count is 5")]
        [MaxFilesSize(10 * 1024 * 1024, ErrorMessage = "Too big file size. Max size is 10MB")]
        [FilesVerifyExtensions("jpg,jpeg,jpe,jif,jfif,png", ErrorMessage = "Incorrect image extension. Choose one of these: jpg, jpeg, jpe, jif, jfif, png")]
        [MaxEnumerableSize(5, ErrorMessage = "Maximum images count is 5")]
        public IEnumerable<IFormFile> Images { get; set; } = new List<IFormFile>();

        [Required(ErrorMessage = "Car brand is required")]
        [Display(Name = "Car brand")]
        [NotZeroValue(ErrorMessage = "You have to chose car brand")]
        public int CarBrandId { get; set; }
        
        [Required(ErrorMessage = "Car model is required")]
        [Display(Name = "Car model")]
        [NotZeroValue(ErrorMessage = "You have to chose car model")]
        public int CarModelId { get; set; }
        
        [Required(ErrorMessage = "Year of production is required")] 
        [Display(Name = "Year of production")]
        [NotZeroValue(ErrorMessage = "You have to chose year of production")]
        public int YearOfProduction { get; set; }

        [Required(ErrorMessage = "Mileage is required")]
        [Display(Name = "Mileage (thousand km.)")]
        [NotZeroValue(ErrorMessage = "You have to enter Mileage")]
        [Range(0, 999)]
        public int Mileage { get; set; }

        [Required(ErrorMessage = "Region is required")]
        [NotZeroValue(ErrorMessage = "You have to chose region")]
        public Region Region { get; set; }

        [Required(ErrorMessage = "Transmission is required")]
        [NotZeroValue(ErrorMessage = "You have to chose transmission")]
        public Transmission Transmission { get; set; }

        [Required(ErrorMessage = "Fuel is required")]
        [NotZeroValue(ErrorMessage = "You have to chose fuel")]
        public Fuel Fuel { get; set; }

        [Display(Name = "Engine capacity (in liters)")]
        public string? EngineCapacity { get; set; }

        [Required(ErrorMessage = "Wheel drive is required")]
        [Display(Name = "Wheel drive")]
        [NotZeroValue(ErrorMessage = "You have to chose wheel drive")]
        public WheelDrive WheelDrive { get; set; }

        [Required(ErrorMessage = "Color is required")]
        [NotZeroValue(ErrorMessage = "You have to chose color")]
        public Color Color { get; set; }

        [Required(ErrorMessage = "Number of seats is required")]
        [Display(Name = "Number of seats")]
        [NotZeroValue(ErrorMessage = "You have to chose number of seats")]
        [Range(1, 60)]
        public int NumberOfSeats { get; set; }

        [MaxLength(150, ErrorMessage = "The maximum title length is 150 characters long")]
        [Display(Name = "Additional options (through comma) (optional, leave this field blank if you don't need it)")]
        public string? AdditionalOptions { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(3, ErrorMessage = "The minimum title length must be at least 3 characters long")]
        [MaxLength(50, ErrorMessage = "The maximum title length is 50 characters long")]
        [Display(Name = "Ad title (provide basic details)")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Text is required")]
        [MinLength(10, ErrorMessage = "The minimum text length must be at least 10 characters long")]
        [MaxLength(700, ErrorMessage = "The maximum text length is 500 characters long")]
        [Display(Name = "Ad text (car description)")]
        public string Text { get; set; } = null!;
        
        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Price (in numbers) and currency")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [Display(Name = "Currency")]
        [NotZeroValue(ErrorMessage = "You have to chose currency")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "Main photo is required")]
        public string MainImage { get; set; } = null!;

        public List<SelectListItem> CarBrands { get; set; } = new();
        
        public List<SelectListItem> CarModels { get; set; } = new();
        
        public List<SelectListItem> Currencies { get; set; } = new();
    }
}