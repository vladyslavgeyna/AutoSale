using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoSale.Service.Implementations
{
    public class UserReviewService : IUserReviewService
    {
        private readonly IUserReviewRepository _userReviewRepository;

        public UserReviewService(IUserReviewRepository userReviewRepository)
        {
            _userReviewRepository = userReviewRepository;
        }
        
        public async Task<IResponse<List<UserReview>>> GetAllAsync(bool included = false)
        {
            try
            {
                var userReviews = included
                    ? await _userReviewRepository.Select()
                        .Include(ur => ur.UserTo)
                        .Include(ur => ur.UserTo.Image)
                        .Include(ur => ur.UserFrom)
                        .Include(ur => ur.UserFrom.Image)
                        .ToListAsync()
                    : await _userReviewRepository.Select().ToListAsync();
                
                if (!userReviews.Any())
                {
                    return new Response<List<UserReview>>
                    {
                        Data = userReviews,
                        Description = $"User reviews not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<UserReview>>
                {
                    Data = userReviews,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<UserReview>>
                {
                    Description = $"[UserReviewService:GetAllAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<UserReview>> GetByIdAsync(int id, bool included = false)
        {
            try
            {
                var userReview = included
                    ? await _userReviewRepository.Select()
                        .Include(ur => ur.UserTo)
                        .Include(ur => ur.UserTo.Image)
                        .Include(ur => ur.UserFrom)
                        .Include(ur => ur.UserFrom.Image)
                        .FirstOrDefaultAsync(ur => ur.Id == id)
                    : await _userReviewRepository.GetByIdAsync(id);

                if (userReview is null)
                {
                    return new Response<UserReview>
                    {
                        Description = $"User review not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<UserReview>
                {
                    Data = userReview,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<UserReview>
                {
                    Description = $"[UserReviewService:GetByIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public IResponse<IQueryable<UserReview>> GetAllAsQueryable(bool included = false)
        {
            try
            {
                var userReviews = included
                    ? _userReviewRepository.Select()
                        .Include(ur => ur.UserTo)
                        .Include(ur => ur.UserTo.Image)
                        .Include(ur => ur.UserFrom)
                        .Include(ur => ur.UserFrom.Image)
                    : _userReviewRepository.Select();
                
                return new Response<IQueryable<UserReview>>
                {
                    Data = userReviews,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<IQueryable<UserReview>>
                {
                    Description = $"[UserReviewService:GetAllAsQueryable] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<UserReview>> CreateAsync(UserReview userReview)
        {
            try
            {
                userReview = await _userReviewRepository.InsertAsync(userReview);
                
                return new Response<UserReview>
                {
                    Data = userReview,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<UserReview>
                {
                    Description = $"[UserReviewService:CreateAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<List<UserReview>>> GetByUserIdToAsync(string userIdTo, bool included = false)
        {
            try
            {
                var userReviews = included
                    ? await _userReviewRepository.Select()
                        .Include(ur => ur.UserTo)
                        .Include(ur => ur.UserTo.Image)
                        .Include(ur => ur.UserFrom)
                        .Include(ur => ur.UserFrom.Image)
                        .Where(ur => ur.UserIdTo == userIdTo)
                        .ToListAsync()
                    : await _userReviewRepository.Select()
                        .Where(fa => fa.UserIdTo == userIdTo)
                        .ToListAsync();

                if (!userReviews.Any())
                {
                    return new Response<List<UserReview>>
                    {
                        Data = userReviews,
                        Description = $"User reviews not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<UserReview>>
                {
                    Data = userReviews,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<UserReview>>
                {
                    Description = $"[UserReviewService:GetByUserIdToAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<UserReview>> EditAsync(UserReview userReview)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<bool>> RemoveAsync(int id)
        {
            try
            {
                var userReview = await _userReviewRepository.GetByIdAsync(id);
                
                if (userReview is null)
                {
                    return new Response<bool>
                    {
                        Data = false,
                        Description = $"User review not found",
                        Code = ResponseCode.NotFound
                    };
                }

                await _userReviewRepository.DeleteAsync(userReview);
                
                return new Response<bool>
                {
                    Data = true,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<bool>
                {
                    Data = false,
                    Description = $"[UserReviewService:RemoveAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }
    }
}