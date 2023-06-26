using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Domain.ViewModels.CarAd;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AutoSale.Service.Implementations
{
    public class CarAdService : ICarAdService
    {
        private readonly ICarAdRepository _carAdRepository;
        private readonly ICarService _carService;
        private readonly ICarImageService _carImageService;
        private readonly IImageService _imageService;

        public CarAdService(ICarAdRepository carAdRepository, 
            ICarService carService,
            ICarImageService carImageService,
            IImageService imageService)
        {
            _carAdRepository = carAdRepository;
            _carService = carService;
            _carImageService = carImageService;
            _imageService = imageService;
        }

        public async Task<IResponse<List<CarAd>>> GetAllAsync(bool included = false)
        {
            try
            {
                var carAds = included
                    ? await _carAdRepository.Select()
                        .Include(ca => ca.User)
                        .Include(ca => ca.User.Image)
                        .Include(ca => ca.Car)
                        .Include(ca => ca.Car.CarBrand)
                        .Include(ca => ca.Car.CarModel)
                        .Include(ca => ca.Car.Currency)
                        .ToListAsync()
                    : await _carAdRepository.Select().ToListAsync();
                
                if (!carAds.Any())
                {
                    return new Response<List<CarAd>>
                    {
                        Description = $"Car ads not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<CarAd>>
                {
                    Data = carAds,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<CarAd>>
                {
                    Description = $"[CarAdService:GetAllAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarAd>> GetByIdAsync(int id, bool included = false)
        {
            try
            {
                var carAd = included
                    ? await _carAdRepository.Select()
                        .Include(ca => ca.User)
                        .Include(ca => ca.User.Image)
                        .Include(ca => ca.Car)
                        .Include(ca => ca.Car.CarBrand)
                        .Include(ca => ca.Car.CarModel)
                        .Include(ca => ca.Car.Currency)
                        .FirstOrDefaultAsync(c => c.Id == id)
                    : await _carAdRepository.GetByIdAsync(id);

                if (carAd is null)
                {
                    return new Response<CarAd>
                    {
                        Description = $"Car ad not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<CarAd>
                {
                    Data = carAd,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<CarAd>
                {
                    Description = $"[CarAdService:GetByIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<List<CarAd>>> GetByUserIdAsync(string userId, bool included = false)
        {
            try
            {
                var carAds = included
                    ? await _carAdRepository.Select()
                        .Include(ca => ca.User)
                        .Include(ca => ca.User.Image)
                        .Include(ca => ca.Car)
                        .Include(ca => ca.Car.CarBrand)
                        .Include(ca => ca.Car.CarModel)
                        .Include(ca => ca.Car.Currency)
                        .Where(ca => ca.UserId == userId)
                        .ToListAsync()
                    : await _carAdRepository.Select()
                        .Where(ca => ca.UserId == userId)
                        .ToListAsync();

                if (!carAds.Any())
                {
                    return new Response<List<CarAd>>
                    {
                        Description = $"Car ads not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<CarAd>>
                {
                    Data = carAds,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<CarAd>>
                {
                    Description = $"[CarAdService:GetByUserIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }


        public async Task<IResponse<CarAd>> CreateAsync(CreateCarAdViewModel createCarAdViewModel)
        {
            try
            {
                Car car = new()
                {
                    CarModelId = createCarAdViewModel.CarModelId,
                    CarBrandId = createCarAdViewModel.CarBrandId,
                    YearOfProduction = createCarAdViewModel.YearOfProduction,
                    EngineCapacity = createCarAdViewModel.EngineCapacity is null ? null : double.Parse(createCarAdViewModel.EngineCapacity),
                    Fuel = createCarAdViewModel.Fuel,
                    Color = createCarAdViewModel.Color,
                    Transmission = createCarAdViewModel.Transmission,
                    Region = createCarAdViewModel.Region,
                    Price = createCarAdViewModel.Price,
                    CurrencyId = createCarAdViewModel.CurrencyId,
                    WheelDrive = createCarAdViewModel.WheelDrive,
                    NumberOfSeats = createCarAdViewModel.NumberOfSeats,
                    Mileage = createCarAdViewModel.Mileage,
                    AdditionalOptions = createCarAdViewModel.AdditionalOptions
                };

                var carResponse = await _carService.CreateAsync(car);
                
                if (carResponse.Code is not ResponseCode.Ok)
                {
                    return new Response<CarAd>
                    {
                        Description = carResponse.Description,
                        Code = carResponse.Code
                    };
                }

                CarAd carAd = new()
                {
                    CarId = carResponse.Data.Id,
                    Title = createCarAdViewModel.Title,
                    Text = createCarAdViewModel.Text,
                    DateOfCreation = DateTime.Now,
                    IsActive = true,
                    UserId = createCarAdViewModel.UserId
                };

                carAd = await _carAdRepository.InsertAsync(carAd);

                foreach (var image in createCarAdViewModel.Images)
                {
                    var imageResponse = await _imageService.CreateAsync(image);
                    
                    if (imageResponse.Code is not ResponseCode.Ok)
                    {
                        return new Response<CarAd>
                        {
                            Description = imageResponse.Description,
                            Code = imageResponse.Code
                        };
                    }

                    CarImage carImage = new()
                    {
                        CarId = carResponse.Data.Id,
                        ImageId = imageResponse.Data.Id,
                        IsMain = createCarAdViewModel.MainImage == image.FileName
                    };
                    
                    var carImageResponse = await _carImageService.CreateAsync(carImage);

                    if (carImageResponse.Code is not ResponseCode.Ok)
                    {
                        return new Response<CarAd>
                        {
                            Description = carImageResponse.Description,
                            Code = carImageResponse.Code
                        };
                    }
                }

                return new Response<CarAd>
                {
                    Data = carAd,
                    Code = ResponseCode.Ok
                };

            }
            catch (Exception e)
            {
                return new Response<CarAd>
                {
                    Description = $"[CarAdService:CreateAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarAd>> EditAsync(CarAd carAd)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<bool>> RemoveAsync(int id)
        {
            try
            {
                var carAd = await _carAdRepository.GetByIdAsync(id);

                if (carAd is null)
                {
                    return new Response<bool>
                    {
                        Data = false,
                        Description = $"Car ad not found",
                        Code = ResponseCode.NotFound
                    };
                }
                var carId = carAd.CarId;

                var carImageResponse = await _carImageService.GetByCarIdAsync(carId);

                if (carImageResponse.Code is not ResponseCode.Ok)
                {
                    return new Response<bool>
                    {
                        Data = false,
                        Description = carImageResponse.Description,
                        Code = carImageResponse.Code
                    };
                }
                
                await _carAdRepository.DeleteAsync(carAd);

                foreach (var carImage in carImageResponse.Data)
                {
                    var imageId = carImage.ImageId;
                    
                    var removeCarImageResponse = await _carImageService.RemoveAsync(carImage.Id);
                    
                    if (removeCarImageResponse.Code is not ResponseCode.Ok)
                    {
                        return new Response<bool>
                        {
                            Data = false,
                            Description = removeCarImageResponse.Description,
                            Code = removeCarImageResponse.Code
                        };
                    }
                    
                    var removeImageResponse = await _imageService.RemoveAsync(imageId);
                    
                    if (removeImageResponse.Code is not ResponseCode.Ok)
                    {
                        return new Response<bool>
                        {
                            Data = false,
                            Description = removeImageResponse.Description,
                            Code = removeImageResponse.Code
                        };
                    }
                }

                await _carService.RemoveAsync(carId);

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
                    Description = $"[CarAdService:RemoveAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }
    }
}