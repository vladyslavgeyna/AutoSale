using AutoSale.Domain.Models;
using AutoSale.Domain.Response;

namespace AutoSale.Service.Interfaces
{
    public interface ICarBrandService
    {
        Task<IResponse<List<CarBrand>>> GetAllAsync();
        
        Task<IResponse<CarBrand>> GetByIdAsync(int id);
        
        Task<IResponse<CarBrand>> CreateAsync(CarBrand carBrand);
        
        Task<IResponse<CarBrand>> EditAsync(CarBrand carBrand);
        
        Task<IResponse<CarBrand>> RemoveAsync(int id);
    }
}