using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoSale.Service.Implementations
{
    public class CarComparisonService : ICarComparisonService
    {
        private readonly ICarComparisionRepository _carComparisionRepository;

        public CarComparisonService(ICarComparisionRepository carComparisionRepository)
        {
            _carComparisionRepository = carComparisionRepository;
        }
        
        public async Task<IResponse<List<CarComparison>>> GetAllAsync(bool included = false)
        {
            try
            {
                var carComparisons = included
                    ? await _carComparisionRepository.Select()
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
                    : await _carComparisionRepository.Select().ToListAsync();
                
                if (!carComparisons.Any())
                {
                    return new Response<List<CarComparison>>
                    {
                        Data = carComparisons,
                        Description = $"Car comparisons not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<CarComparison>>
                {
                    Data = carComparisons,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<CarComparison>>
                {
                    Description = $"[CarComparisonService:GetAllAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarComparison>> GetByIdAsync(int id, bool included = false)
        {
            try
            {
                var carComparison = included
                    ? await _carComparisionRepository.Select()
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
                    : await _carComparisionRepository.GetByIdAsync(id);

                if (carComparison is null)
                {
                    return new Response<CarComparison>
                    {
                        Description = $"Car comparison not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<CarComparison>
                {
                    Data = carComparison,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<CarComparison>
                {
                    Description = $"[CarComparisonService:GetByIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarComparison>> CreateAsync(CarComparison carComparison)
        {
            try
            {
                carComparison = await _carComparisionRepository.InsertAsync(carComparison);
                
                return new Response<CarComparison>
                {
                    Data = carComparison,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<CarComparison>
                {
                    Description = $"[CarComparisonService:CreateAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<List<CarComparison>>> GetByUserIdAsync(string userId, bool included = false)
        {
            try
            {
                var carComparisons = included
                    ? await _carComparisionRepository.Select()
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
                    : await _carComparisionRepository.Select()
                        .Where(fa => fa.UserId == userId && fa.CarAd.IsActive)
                        .ToListAsync();

                if (!carComparisons.Any())
                {
                    return new Response<List<CarComparison>>
                    {
                        Data = carComparisons,
                        Description = $"Car comparisons not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<CarComparison>>
                {
                    Data = carComparisons,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<CarComparison>>
                {
                    Description = $"[CarComparisonService:GetByUserIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarComparison>> GetByUserIdAndCarAdIdAsync(string userId, int carAdId, bool included = false)
        {
            try
            {
                var carComparison = included
                    ? await _carComparisionRepository.Select()
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
                    : await _carComparisionRepository.Select()
                        .Where(fa => fa.UserId == userId && fa.CarAdId == carAdId)
                        .FirstOrDefaultAsync();

                if (carComparison is null)
                {
                    return new Response<CarComparison>
                    {
                        Description = $"Car comparison not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<CarComparison>
                {
                    Data = carComparison,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<CarComparison>
                {
                    Description = $"[CarComparisonService:GetByUserIdAndCarAdIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarComparison>> EditAsync(CarComparison carComparison)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<bool>> RemoveAsync(int id)
        {
            try
            {
                var carComparison = await _carComparisionRepository.GetByIdAsync(id);
                
                if (carComparison is null)
                {
                    return new Response<bool>
                    {
                        Data = false,
                        Description = $"Car comparison not found",
                        Code = ResponseCode.NotFound
                    };
                }

                await _carComparisionRepository.DeleteAsync(carComparison);
                
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
                    Description = $"[CarComparisonService:RemoveAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }
    }
}