using AutoSale.DAL;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using X.PagedList;

namespace AutoSale.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ICarComparisonService _carComparisonService;
        private readonly IFavoriteAdService _favoriteAdService;
        private readonly ICarAdService _carAdService;
        private readonly IImageService _imageService;
        private readonly IUserReviewService _userReviewService;

        public UserService(UserManager<User> userManager,
            ICarComparisonService carComparisonService,
            IFavoriteAdService favoriteAdService,
            ICarAdService carAdService,
            IImageService imageService,
            IUserReviewService userReviewService)
        {
            _userManager = userManager;
            _carComparisonService = carComparisonService;
            _favoriteAdService = favoriteAdService;
            _carAdService = carAdService;
            _imageService = imageService;
            _userReviewService = userReviewService;
        }
        
        public async Task<IResponse<IdentityResult>> RemoveAsync(User user)
        {
            try
            {
                string errors = "";
                
                List<IdentityResult> identityResults = new();

                var roles = await _userManager.GetRolesAsync(user);
                
                identityResults.Add(await _userManager.RemoveFromRolesAsync(user, roles));
                
                var claims = await _userManager.GetClaimsAsync(user);
                
                identityResults.Add(await _userManager.RemoveClaimsAsync(user, claims));

                var logins = await _userManager.GetLoginsAsync(user);
                
                foreach (var login in logins)
                {
                    identityResults.Add(await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey));
                }

                if (identityResults.Any(ir => !ir.Succeeded))
                {
                    errors = "";
                    foreach (var identityResult in identityResults)
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            errors += error.Description + Environment.NewLine;
                        }
                    }
                    return new Response<IdentityResult>
                    {
                        Description = errors,
                        Code = ResponseCode.Error
                    };
                }

                var carComparisonResponse = await _carComparisonService.GetByUserIdAsync(user.Id);

                if (carComparisonResponse.Code is ResponseCode.InternalServerError)
                {
                    return new Response<IdentityResult>
                    {
                        Description = carComparisonResponse.Description,
                        Code = ResponseCode.InternalServerError
                    };
                }

                foreach (var carComparison in carComparisonResponse.Data)
                {
                    var deleteCarComparisonResponse = await _carComparisonService.RemoveAsync(carComparison.Id);
                    if (deleteCarComparisonResponse.Code is not ResponseCode.Ok)
                    {
                        return new Response<IdentityResult>
                        {
                            Description = deleteCarComparisonResponse.Description,
                            Code = ResponseCode.Error
                        };
                    }
                }
                
                var favoriteAdResponse = await _favoriteAdService.GetByUserIdAsync(user.Id);

                if (favoriteAdResponse.Code is ResponseCode.InternalServerError)
                {
                    return new Response<IdentityResult>
                    {
                        Description = favoriteAdResponse.Description,
                        Code = ResponseCode.InternalServerError
                    };
                }

                foreach (var favoriteAd in favoriteAdResponse.Data)
                {
                    var deleteFavoriteAdResponse = await _favoriteAdService.RemoveAsync(favoriteAd.Id);
                    if (deleteFavoriteAdResponse.Code is not ResponseCode.Ok)
                    {
                        return new Response<IdentityResult>
                        {
                            Description = deleteFavoriteAdResponse.Description,
                            Code = ResponseCode.Error
                        };
                    }
                }
                
                var carAdResponse = await _carAdService.GetByUserIdAsync(user.Id);

                if (carAdResponse.Code is ResponseCode.InternalServerError)
                {
                    return new Response<IdentityResult>
                    {
                        Description = carAdResponse.Description,
                        Code = ResponseCode.InternalServerError
                    };
                }

                foreach (var carAd in carAdResponse.Data)
                {
                    var deleteCarAdResponse = await _carAdService.RemoveAsync(carAd.Id);
                    if (deleteCarAdResponse.Code is not ResponseCode.Ok)
                    {
                        return new Response<IdentityResult>
                        {
                            Description = deleteCarAdResponse.Description,
                            Code = ResponseCode.Error
                        };
                    }
                }

                if (user.ImageId is not null)
                {
                    var deleteImageResponse = await _imageService.RemoveAsync((int)user.ImageId);
                    if (deleteImageResponse.Code is not ResponseCode.Ok)
                    {
                        return new Response<IdentityResult>
                        {
                            Description = deleteImageResponse.Description,
                            Code = ResponseCode.Error
                        };
                    }
                }


                var userReviewResponse = _userReviewService.GetAllAsQueryable();

                if (userReviewResponse.Code is not ResponseCode.Ok)
                {
                    return new Response<IdentityResult>
                    {
                        Description = userReviewResponse.Description,
                        Code = ResponseCode.Error
                    };
                }
                
                var userReviews = await userReviewResponse.Data
                    .Where(ur => ur.UserIdFrom == user.Id || ur.UserIdTo == user.Id)
                    .ToListAsync();

                foreach (var userReview in userReviews)
                {
                    var deleteUserReviewResponse = await _userReviewService.RemoveAsync(userReview.Id);
                    if (deleteUserReviewResponse.Code is not ResponseCode.Ok)
                    {
                        return new Response<IdentityResult>
                        {
                            Description = deleteUserReviewResponse.Description,
                            Code = ResponseCode.Error
                        };
                    }
                }

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return new Response<IdentityResult>
                    {
                        Data = result,
                        Code = ResponseCode.Ok
                    };
                }
                
                errors = "";
                
                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }
                
                return new Response<IdentityResult>
                {
                    Description = errors,
                    Code = ResponseCode.Error
                };
            }
            catch (Exception e)
            {
                return new Response<IdentityResult>
                {
                    Description = $"[UserService:RemoveAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }
    }
}