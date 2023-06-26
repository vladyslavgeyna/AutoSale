using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoSale.Service.Implementations
{
    public class CarBrandService : ICarBrandService
    {
        private readonly ICarBrandRepository _carBrandRepository;

        public CarBrandService(ICarBrandRepository carBrandRepository)
        {
            _carBrandRepository = carBrandRepository;
        }
        
        public async Task<IResponse<List<CarBrand>>> GetAllAsync()
        {
            try
            {
                var carBrands = await _carBrandRepository.Select().ToListAsync();
                
                if (!carBrands.Any())
                {
                    return new Response<List<CarBrand>>
                    {
                        Description = $"Car brands not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<CarBrand>>
                {
                    Data = carBrands,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<CarBrand>>
                {
                    Description = $"[CarBrandService:GetAllAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<CarBrand>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<CarBrand>> CreateAsync(CarBrand carBrand)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<CarBrand>> EditAsync(CarBrand carBrand)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<CarBrand>> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}