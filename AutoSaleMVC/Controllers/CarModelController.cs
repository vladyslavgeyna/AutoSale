using AutoSale.Domain.Enum;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoSaleMVC.Controllers
{
    public class CarModelController : Controller
    {
        private readonly ICarModelService _carModelService;

        public CarModelController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(int carBrandId = 1)
        {
            var response = await _carModelService.GetByCarBrandIdAsync(carBrandId);
            
            if (response.Code is not ResponseCode.Ok)
            {
                return BadRequest();
            }

            return Json(response.Data);
        }
        
        
    }
}