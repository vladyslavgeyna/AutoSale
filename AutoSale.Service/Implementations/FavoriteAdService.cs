using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoSale.Service.Implementations
{
    public class FavoriteAdService : IFavoriteAdService
    {
        private readonly IFavoriteAdRepository _favoriteAdRepository;

        public FavoriteAdService(IFavoriteAdRepository favoriteAdRepository)
        {
            _favoriteAdRepository = favoriteAdRepository;
        }
        
        public async Task<IResponse<List<FavoriteAd>>> GetAllAsync(bool included = false)
        {
            try
            {
                var favoriteAds = included
                    ? await _favoriteAdRepository.Select()
                        .Include(fa => fa.User)
                        .Include(fa => fa.User.Image)
                        .Include(fa => fa.CarAd)
                        .Include(fa => fa.CarAd.User)
                        .Include(fa => fa.CarAd.User.Image)
                        .Include(fa => fa.CarAd.Car)
                        .Include(fa => fa.CarAd.Car.CarBrand)
                        .Include(fa => fa.CarAd.Car.CarModel)
                        .Include(fa => fa.CarAd.Car.Currency)
                        .ToListAsync()
                    : await _favoriteAdRepository.Select().ToListAsync();
                
                if (!favoriteAds.Any())
                {
                    return new Response<List<FavoriteAd>>
                    {
                        Data = favoriteAds,
                        Description = $"Favorite ads not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<FavoriteAd>>
                {
                    Data = favoriteAds,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<FavoriteAd>>
                {
                    Description = $"[FavoriteAdService:GetAllAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<FavoriteAd>> GetByIdAsync(int id, bool included = false)
        {
            try
            {
                var favoriteAd = included
                    ? await _favoriteAdRepository.Select()
                        .Include(fa => fa.User)
                        .Include(fa => fa.User.Image)
                        .Include(fa => fa.CarAd)
                        .Include(fa => fa.CarAd.User)
                        .Include(fa => fa.CarAd.User.Image)
                        .Include(fa => fa.CarAd.Car)
                        .Include(fa => fa.CarAd.Car.CarBrand)
                        .Include(fa => fa.CarAd.Car.CarModel)
                        .Include(fa => fa.CarAd.Car.Currency)
                        .FirstOrDefaultAsync(fa => fa.Id == id)
                    : await _favoriteAdRepository.GetByIdAsync(id);

                if (favoriteAd is null)
                {
                    return new Response<FavoriteAd>
                    {
                        Description = $"Favorite ad not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<FavoriteAd>
                {
                    Data = favoriteAd,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<FavoriteAd>
                {
                    Description = $"[FavoriteAdService:GetByIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<FavoriteAd>> CreateAsync(FavoriteAd favoriteAd)
        {
            try
            {
                favoriteAd = await _favoriteAdRepository.InsertAsync(favoriteAd);
                
                return new Response<FavoriteAd>
                {
                    Data = favoriteAd,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<FavoriteAd>
                {
                    Description = $"[FavoriteAdService:CreateAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<List<FavoriteAd>>> GetByUserIdAsync(string userId, bool included = false)
        {
            try
            {
                var favoriteAds = included
                    ? await _favoriteAdRepository.Select()
                        .Include(fa => fa.User)
                        .Include(fa => fa.User.Image)
                        .Include(fa => fa.CarAd)
                        .Include(fa => fa.CarAd.User)
                        .Include(fa => fa.CarAd.User.Image)
                        .Include(fa => fa.CarAd.Car)
                        .Include(fa => fa.CarAd.Car.CarBrand)
                        .Include(fa => fa.CarAd.Car.CarModel)
                        .Include(fa => fa.CarAd.Car.Currency)
                        .Where(fa => fa.UserId == userId && fa.CarAd.IsActive)
                        .ToListAsync()
                    : await _favoriteAdRepository.Select()
                        .Where(fa => fa.UserId == userId && fa.CarAd.IsActive)
                        .ToListAsync();

                if (!favoriteAds.Any())
                {
                    return new Response<List<FavoriteAd>>
                    {
                        Data = favoriteAds,
                        Description = $"Favorite ads not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<FavoriteAd>>
                {
                    Data = favoriteAds,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<FavoriteAd>>
                {
                    Description = $"[FavoriteAdService:GetByUserIdToAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<FavoriteAd>> GetByUserIdAndCarAdIdAsync(string userId, int carAdId, bool included = false)
        {
            try
            {
                var favoriteAd = included
                    ? await _favoriteAdRepository.Select()
                        .Include(fa => fa.User)
                        .Include(fa => fa.User.Image)
                        .Include(fa => fa.CarAd)
                        .Include(fa => fa.CarAd.User)
                        .Include(fa => fa.CarAd.User.Image)
                        .Include(fa => fa.CarAd.Car)
                        .Include(fa => fa.CarAd.Car.CarBrand)
                        .Include(fa => fa.CarAd.Car.CarModel)
                        .Include(fa => fa.CarAd.Car.Currency)
                        .Where(fa => fa.UserId == userId && fa.CarAdId == carAdId)
                        .FirstOrDefaultAsync()
                    : await _favoriteAdRepository.Select()
                        .Where(fa => fa.UserId == userId && fa.CarAdId == carAdId)
                        .FirstOrDefaultAsync();

                if (favoriteAd is null)
                {
                    return new Response<FavoriteAd>
                    {
                        Description = $"Favorite ad not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<FavoriteAd>
                {
                    Data = favoriteAd,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<FavoriteAd>
                {
                    Description = $"[FavoriteAdService:GetByUserIdAndCarAdIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<FavoriteAd>> EditAsync(FavoriteAd favoriteAd)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<bool>> RemoveAsync(int id)
        {
            try
            {
                var favoriteAd = await _favoriteAdRepository.GetByIdAsync(id);
                
                if (favoriteAd is null)
                {
                    return new Response<bool>
                    {
                        Data = false,
                        Description = $"Favorite ad not found",
                        Code = ResponseCode.NotFound
                    };
                }

                await _favoriteAdRepository.DeleteAsync(favoriteAd);
                
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
                    Description = $"[FavoriteAdService:RemoveAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<int>> GetCountOfFavoriteAdsByCarAdId(int carAdId)
        {
            try
            {
                var count = await _favoriteAdRepository.Select()
                    .Where(fa => fa.CarAdId == carAdId)
                    .CountAsync();
                
                return new Response<int>
                {
                    Data = count,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<int>
                {
                    Description = $"[FavoriteAdService:GetCountOfFavoriteAdsByCarAdId] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }
    }
}