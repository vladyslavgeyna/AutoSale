using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoSale.Service.Implementations
{
    public class CarImageService : ICarImageService
    {
        private readonly ICarImageRepository _carImageRepository;

        public CarImageService(ICarImageRepository carImageRepository)
        {
            _carImageRepository = carImageRepository;
        }

        public async Task<IResponse<CarImage>> GetMainCarImageByCarIdAsync(int carId, bool included = false)
        {
            try
            {
                var carImage = included
                    ? await _carImageRepository.Select()
                        .Include(ci => ci.Image)
                        .Include(ci => ci.Car)
                        .Include(ci => ci.Car.CarBrand)
                        .Include(ci => ci.Car.CarModel)
                        .Include(ci => ci.Car.Currency)
                        .Where(ci => ci.CarId == carId)
                        .FirstOrDefaultAsync(ci => ci.IsMain)
                    : await _carImageRepository.Select()
                        .Where(ci => ci.CarId == carId)
                        .FirstOrDefaultAsync(ci => ci.IsMain);
                
                if (carImage is null)
                {
                    return new Response<CarImage>
                    {
                        Description = "Car image not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<CarImage>
                {
                    Data = carImage,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<CarImage>
                {
                    Description = $"[CarImageService:GetMainCarImageByCarIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }
        
        public async Task<IResponse<List<CarImage>>> GetAllAsync(bool included = false)
        {
            try
            {
                var carImages = included
                    ? await _carImageRepository.Select()
                        .Include(ci => ci.Image)
                        .Include(ci => ci.Car)
                        .Include(ci => ci.Car.CarBrand)
                        .Include(ci => ci.Car.CarModel)
                        .Include(ci => ci.Car.Currency)
                        .ToListAsync()
                    : await _carImageRepository.Select().ToListAsync();

                if (!carImages.Any())
                {
                    return new Response<List<CarImage>>
                    {
                        Description = $"Car images not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<CarImage>>
                {
                    Data = carImages,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<CarImage>>
                {
                    Description = $"[CarImageService:GetAllAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarImage>> GetByIdAsync(int id, bool included = false)
        {
            try
            {
                var carImage = included
                    ? await _carImageRepository.Select()
                        .Include(ci => ci.Image)
                        .Include(ci => ci.Car)
                        .Include(ci => ci.Car.CarBrand)
                        .Include(ci => ci.Car.CarModel)
                        .Include(ci => ci.Car.Currency)
                        .FirstOrDefaultAsync(ci => ci.Id == id)
                    : await _carImageRepository.GetByIdAsync(id);
                
                if (carImage is null)
                {
                    return new Response<CarImage>
                    {
                        Description = $"Car image not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<CarImage>
                {
                    Data = carImage,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<CarImage>
                {
                    Description = $"[CarImageService:GetByIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<List<CarImage>>> GetByCarIdAsync(int carId, bool included = false)
        {
            try
            {
                var carImages = included
                    ? await _carImageRepository.Select()
                        .Include(ci => ci.Image)
                        .Include(ci => ci.Car)
                        .Include(ci => ci.Car.CarBrand)
                        .Include(ci => ci.Car.CarModel)
                        .Include(ci => ci.Car.Currency)
                        .Where(ci => ci.CarId == carId)
                        .ToListAsync()
                    : await _carImageRepository.Select()
                        .Where(ci => ci.CarId == carId)
                        .ToListAsync();
                
                if (!carImages.Any())
                {
                    return new Response<List<CarImage>>
                    {
                        Description = $"Car images not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<CarImage>>
                {
                    Data = carImages,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<CarImage>>
                {
                    Description = $"[CarImageService:GetByCarIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarImage>> CreateAsync(CarImage carImage)
        {
            try
            {
                carImage = await _carImageRepository.InsertAsync(carImage);
                
                return new Response<CarImage>
                {
                    Data = carImage,
                    Code = ResponseCode.Ok
                };            
            }
            catch (Exception e)
            {
                return new Response<CarImage>
                {
                    Description = $"[CarImageService:CreateAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarImage>> EditAsync(CarImage carImage)
        {
            try
            {
                carImage = await _carImageRepository.UpdateAsync(carImage);
                
                return new Response<CarImage>
                {
                    Data = carImage,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<CarImage>
                {
                    Description = $"[CarService:EditAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }      
        }

        public async Task<IResponse<bool>> RemoveAsync(int id)
        {
            try
            {
                var carImage = await _carImageRepository.GetByIdAsync(id);
                
                if (carImage is null)
                {
                    return new Response<bool>
                    {
                        Data = false,
                        Description = $"Car image not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                // todo може тут треба видалити через imageService саме зображення або все довбанути в caradservice
                
                await _carImageRepository.DeleteAsync(carImage);
                
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
                    Description = $"[CarImageService:RemoveAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }
    }
}