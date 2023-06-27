using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoSale.Service.Implementations
{
    public class CarModelService : ICarModelService
    {
        private readonly ICarModelRepository _carModelRepository;

        public CarModelService(ICarModelRepository carModelRepository)
        {
            _carModelRepository = carModelRepository;
        }
        
        public async Task<IResponse<List<CarModel>>> GetAllAsync(bool included = false)
        {
            try
            {
                var carModels = included
                    ? await _carModelRepository.Select()
                        .Include(cm => cm.CarBrand)
                        .ToListAsync()
                    : await _carModelRepository.Select().ToListAsync();
                
                if (!carModels.Any())
                {
                    return new Response<List<CarModel>>
                    {
                        Data = carModels,
                        Description = $"Car models not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<CarModel>>
                {
                    Data = carModels,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<CarModel>>
                {
                    Description = $"[CarModelService:GetAllAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarModel>> GetByIdAsync(int id, bool included = false)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<List<CarModel>>> GetByCarBrandIdAsync(int carBrandId, bool included = false)
        {
            try
            {
                var carModels = included
                    ? await _carModelRepository.Select()
                        .Include(cm => cm.CarBrand)
                        .Where(cm => cm.CarBrandId == carBrandId)
                        .ToListAsync()
                    : await _carModelRepository.Select()
                        .Where(cm => cm.CarBrandId == carBrandId)
                        .ToListAsync();

                if (!carModels.Any())
                {
                    return new Response<List<CarModel>>
                    {
                        Data = carModels,
                        Description = $"Car models not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<CarModel>>
                {
                    Data = carModels,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<CarModel>>
                {
                    Description = $"[CarModelService:GetByCarBrandIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarModel>> CreateAsync(CarModel carModel)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<CarModel>> EditAsync(CarModel carModel)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<CarModel>> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}