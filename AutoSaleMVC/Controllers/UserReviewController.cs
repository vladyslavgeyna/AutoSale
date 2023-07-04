using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.ViewModels.UserReview;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoSaleMVC.Controllers
{
    public class UserReviewController : Controller
    {
        private readonly IUserReviewService _userReviewService;
        private readonly UserManager<User> _userManager;

        public UserReviewController(IUserReviewService userReviewService,
            UserManager<User> userManager)
        {
            _userReviewService = userReviewService;
            _userManager = userManager;
        }
        
        public new async Task<IActionResult> View(string id)
        {
            var userReviewsResponse = await _userReviewService.GetByUserIdToAsync(id, true);

            if (userReviewsResponse.Code is ResponseCode.InternalServerError)
            {
                return base.View("Error");
            }

            var userTo = await _userManager.FindByIdAsync(id);

            if (userTo is null)
            {
                return base.View("Error");
            }

            var isAuthenticated = User.Identity.IsAuthenticated;
            
            var currentUserId = isAuthenticated
                ? (await _userManager.FindByNameAsync(User.Identity.Name)).Id
                : null;
            
            ViewUserReviewViewModel viewUserReviewViewModel = new()
            {
                UserReviews = userReviewsResponse.Data,
                IsAuthenticated = isAuthenticated,
                CurrentUserId = currentUserId,
                UserTo = userTo
            };
            return base.View(viewUserReviewViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(string id)
        {
            var userTo = await _userManager.FindByIdAsync(id);

            if (userTo is null)
            {
                return base.View("Error");
            }

            CreateUserReviewViewModel createUserReviewViewModel = new()
            {
                UserTo = userTo,
                UserIdTo = id
            };

            return View(createUserReviewViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateUserReviewViewModel createUserReviewViewModel)
        {
            if (!ModelState.IsValid)
            {
                var userTo = await _userManager.FindByIdAsync(createUserReviewViewModel.UserIdTo);

                if (userTo is null)
                {
                    return base.View("Error");
                }

                createUserReviewViewModel.UserTo = userTo;
                
                return View(createUserReviewViewModel);
            }

            UserReview userReview = new()
            {
                Text = createUserReviewViewModel.Text,
                Title = createUserReviewViewModel.Title,
                UserIdTo = createUserReviewViewModel.UserIdTo,
                UserIdFrom = (await _userManager.FindByNameAsync(User.Identity.Name)).Id,
                DateOfCreation = DateTime.Now
            };

            var response = await _userReviewService.CreateAsync(userReview);

            if (response.Code is not ResponseCode.Ok)
            {
                return base.View("Error");
            }

            return RedirectToAction("View", new { id = createUserReviewViewModel.UserIdTo });

        }


    }
}