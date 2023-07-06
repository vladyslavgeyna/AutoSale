using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Implementation;

namespace AutoSaleMVC.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;
        private readonly UserManager<User> _userManager;

        public ImageController(IImageService imageService,
            UserManager<User> userManager)
        {
            _imageService = imageService;
            _userManager = userManager;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteCurrentUserImage()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (currentUser is null)
            {
                return BadRequest();
            }

            if (currentUser.ImageId is null)
            {
                return BadRequest();
            }

            var deleteImageResponse = await _imageService.RemoveAsync((int)currentUser.ImageId);

            if (deleteImageResponse.Code is not ResponseCode.Ok)
            {
                return BadRequest();
            }

            return Json(new { });
        }
    }
}