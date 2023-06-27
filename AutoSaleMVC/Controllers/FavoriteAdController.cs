using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AutoSaleMVC.Controllers
{
    public class FavoriteAdController : Controller
    {
        private readonly IFavoriteAdService _favoriteAdService;

        public FavoriteAdController(IFavoriteAdService favoriteAdService)
        {
            _favoriteAdService = favoriteAdService;
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
        
    }
    
    
}