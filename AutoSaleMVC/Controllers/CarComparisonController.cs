using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.ViewModels.CarComparison;
using AutoSale.Domain.ViewModels.FavoriteAd;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoSaleMVC.Controllers
{
    public class CarComparisonController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICarComparisonService _carComparisonService;

        public CarComparisonController(UserManager<User> userManager,
            ICarComparisonService carComparisonService)
        {
            _userManager = userManager;
            _carComparisonService = carComparisonService;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Toggle(string userId, int carAdId)
        {
            var getCarComparisonsResponse = await _carComparisonService.GetByUserIdAndCarAdIdAsync(userId, carAdId);

            if (getCarComparisonsResponse.Code is ResponseCode.Ok)
            {
                var deleteCarComparisonResponse = await _carComparisonService.RemoveAsync(getCarComparisonsResponse.Data.Id);

                if (deleteCarComparisonResponse.Code is not ResponseCode.Ok)
                {
                    return BadRequest();
                }
                
                return Json(new { isExist = false });
            }
            else if (getCarComparisonsResponse.Code is ResponseCode.NotFound)
            {
                CarComparison carComparison = new()
                {
                    UserId = userId,
                    CarAdId = carAdId
                };
                
                var createCarComparisonResponse = await _carComparisonService.CreateAsync(carComparison);
                
                if (createCarComparisonResponse.Code is not ResponseCode.Ok)
                {
                    return BadRequest();
                }
                
                return Json(new { isExist = true });
            }
            else
            {
                return BadRequest();
            }
        }
        
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {

            var currentUserId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;

            var carComparisonsResponse = await _carComparisonService.GetByUserIdAsync(currentUserId, true);

            if (carComparisonsResponse.Code is ResponseCode.InternalServerError)
            {
                return View("Error");
            }

            IndexCarComparisonViewModel indexCarComparisonViewModel = new()
            {
                CurrentUserId = currentUserId,
                CarComparisons = carComparisonsResponse.Data
            };

            return View(indexCarComparisonViewModel);
        }
        
        
        
    }
}