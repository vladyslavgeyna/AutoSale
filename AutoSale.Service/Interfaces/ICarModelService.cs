using AutoSale.Domain.Models;
using AutoSale.Domain.Response;

namespace AutoSale.Service.Interfaces
{
    public interface ICarModelService
    {
        Task<IResponse<List<CarModel>>> GetAllAsync(bool included = false);
        
        Task<IResponse<CarModel>> GetByIdAsync(int id, bool included = false);
        
        Task<IResponse<List<CarModel>>> GetByCarBrandIdAsync(int carBrandId, bool included = false);
        
        Task<IResponse<CarModel>> CreateAsync(CarModel carModel);
        
        Task<IResponse<CarModel>> EditAsync(CarModel carModel);
        
        Task<IResponse<CarModel>> RemoveAsync(int id);
    }
}