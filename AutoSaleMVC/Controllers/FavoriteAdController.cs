using AutoSale.Domain.Enum;
using AutoSale.Domain.Enum.Car;
using AutoSale.Domain.Helpers;
using AutoSale.Domain.Models;
using AutoSale.Domain.ViewModels.FavoriteAd;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoSaleMVC.Controllers
{
    public class FavoriteAdController : Controller
    {
        private readonly IFavoriteAdService _favoriteAdService;
        private readonly UserManager<User> _userManager;

        public FavoriteAdController(IFavoriteAdService favoriteAdService,
            UserManager<User> userManager)
        {
            _favoriteAdService = favoriteAdService;
            _userManager = userManager;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Toggle(string userId, int carAdId)
        {
            var getFavoriteAdResponse = await _favoriteAdService.GetByUserIdAndCarAdIdAsync(userId, carAdId);

            if (getFavoriteAdResponse.Code is ResponseCode.Ok)
            {
                var deleteFavoriteAdResponse = await _favoriteAdService.RemoveAsync(getFavoriteAdResponse.Data.Id);

                if (deleteFavoriteAdResponse.Code is not ResponseCode.Ok)
                {
                    return BadRequest();
                }
                
                return Json(new { isExist = false });
            }
            else if (getFavoriteAdResponse.Code is ResponseCode.NotFound)
            {
                FavoriteAd favoriteAd = new()
                {
                    UserId = userId,
                    CarAdId = carAdId
                };
                
                var createFavoriteAdResponse = await _favoriteAdService.CreateAsync(favoriteAd);
                
                if (createFavoriteAdResponse.Code is not ResponseCode.Ok)
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

            var favoriteAdsResponse = await _favoriteAdService.GetByUserIdAsync(currentUserId, true);

            if (favoriteAdsResponse.Code is ResponseCode.InternalServerError)
            {
                return View("Error");
            }

            IndexFavoriteAdViewModel indexFavoriteAdViewModel = new()
            {
                CurrentUserId = currentUserId,
                FavoriteAds = favoriteAdsResponse.Data
            };

            return View(indexFavoriteAdViewModel);
        }
        
        
    }
}